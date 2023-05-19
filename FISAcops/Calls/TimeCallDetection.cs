using FISAcops.CheckIns;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Windows;

namespace FISAcops
{
    public class TimeCallDetection
    {
        private readonly Thread detectionThread;
        private bool stopDetection;

        private readonly List<CheckIn> CheckInList = new();
        private readonly List<int> Codes = new();
        private readonly List<DateTime> DeleteTime = new();

        private readonly List<Result> resultList = new();


        public TimeCallDetection()
        {
            // Accéder à l'instance du Checker
            checker = Checker.Instance;

            detectionThread = new Thread(DetectionLoop)
            {
                IsBackground = true
            };
            detectionThread.SetApartmentState(ApartmentState.STA); // Configure le thread en mode STA
            stopDetection = false;
        }


        public void StartDetection()
        {
            stopDetection = false;
            detectionThread.Start();
            StartChecker();
        }

        public void StopDetection()
        {
            stopDetection = true;
            detectionThread.Join();
        }

        private int GenerateCode()
        {
            int newCode = new Random().Next(1, 100001);
            while (Codes.Contains(newCode))
            {
                newCode = new Random().Next(1, 100001);
            }
            return newCode;
        }

        private void DeleteStudentFromDetection(int i)
        {
            for (int resultId = 0; resultId < resultList.Count; resultId++)
            {
                for (int studentId = 0; studentId < resultList[resultId].StudentsWithStateList.Count; studentId++)
                {
                    if (resultList[resultId].StudentsWithStateList[studentId].Mail == CheckInList[i].student.Mail)
                    {
                        if (resultList[resultId].StudentsWithStateList[studentId].State == "Controle")
                        {
                            resultList[resultId].StudentsWithStateList[studentId].UpdateState("Absent");
                        }
                    }
                }
            }
            Codes.RemoveAt(i);
            CheckInList.RemoveAt(i);
            DeleteTime.RemoveAt(i);
        }

        

        private void DetectionLoop()
        {

            while (!stopDetection)
            {
                // Obtenir les appels pour la date actuelle
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                List<Call> calls = CallsService.LoadCallsForSelectedDate(currentDate);

                DateTime currentDateTime = DateTime.Now;

                bool show = false;
                // Vérifier si la pop-up doit être affichée pour chaque appel
                foreach (Call call in calls)
                {
                    DateTime callDateTime = ParseDateTime(call.Date, call.Time);

                    DateTime callDateTimePlusOneMinute = callDateTime.AddMinutes(1);


                    if (DeleteTime.Count > 0)
                    {
                        while (currentDateTime >= DeleteTime[0])
                        {
                            DeleteStudentFromDetection(0);
                            if (DeleteTime.Count == 0)
                            {
                                break;
                            }
                        }
                    }

                    for (int i = resultList.Count - 1; i >= 0; i--)
                    {
                        Result result = resultList[i];
                        if (result.IsResultDone())
                        {
                            string jsonFileName = currentDate.Replace("/", "-");
                            List<Result> datedResults = ResultsService.LoadResultsFromJson(jsonFileName);
                            datedResults.Add(result);
                            ResultsService.SaveResultsToJson(datedResults, jsonFileName);
                            resultList.RemoveAt(i);
                        }
                    }



                    if (currentDateTime >= callDateTime && currentDateTime < callDateTimePlusOneMinute)
                    {
                        resultList.Add(new Result(call.Time, call.GroupName, call.StudentsWithState));
                        foreach (StudentWithState student in call.StudentsWithState)
                        {
                            if (student.State == "Controle")
                            {
                                StudentWithCode studentWithCode = (StudentWithCode)StudentFactory.CreateStudent(
                                    student.Nom, 
                                    student.Prenom, 
                                    student.Mail, 
                                    student.Promotion, 
                                    GenerateCode()
                                );
                                CheckInList.Add(new CheckIn(studentWithCode));
                                Codes.Add(studentWithCode.Code);
                                DeleteTime.Add(callDateTime.AddMinutes(1));
                            }
                        }
                        show = true;
                    }
                }
                if (show)
                {
                    ShowPopUp();
                }

                // Attendre avant de vérifier à nouveau
                Thread.Sleep(60000); // Attendre 1 minute
            }
        }

        private static DateTime ParseDateTime(string date, string time)
        {
            string dateTimeString = $"{date} {time}";
            return DateTime.ParseExact(dateTimeString, "dd/MM/yyyy HH:mm", null);
        }

        // serveur sur un thread -------------------------------------------------------------------
        private bool CheckerStarted = false;
        private readonly Checker checker;
        Thread? updateThread;

        private void StartChecker()
        {
            CheckerStarted = true;
            updateThread = new(() =>
            {
                while (CheckerStarted)
                {
                    string receivedMessage = checker.ReceivedMessage;
                    if (!string.IsNullOrEmpty(receivedMessage))
                    {
                        try
                        {
                            if (Application.Current != null)
                            {
                                if (!Application.Current.Dispatcher.HasShutdownStarted)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        if (Checker.LastClient != null)
                                        {
                                            if (int.TryParse(receivedMessage, out int enteredCode))
                                            {
                                                bool noCode = true;
                                                foreach (CheckIn checkIn in CheckInList)
                                                {
                                                    if (checkIn.IsCodeGood(enteredCode))
                                                    {
                                                        //mise à jour du state d'un student
                                                        for (int resultId = 0; resultId < resultList.Count; resultId++)
                                                        {
                                                            for (int studentId = 0; studentId < resultList[resultId].StudentsWithStateList.Count; studentId++)
                                                            {
                                                                if (resultList[resultId].StudentsWithStateList[studentId].Mail == checkIn.student.Mail)
                                                                {
                                                                    if (resultList[resultId].StudentsWithStateList[studentId].State == "Controle")
                                                                    {
                                                                        resultList[resultId].StudentsWithStateList[studentId].UpdateState("Présent");
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        Checker.SendResponseToClient(Checker.LastClient, checkIn.CodeMessage(enteredCode));
                                                        noCode = false;
                                                    }
                                                }
                                                if (noCode)
                                                {
                                                    Checker.SendResponseToClient(Checker.LastClient, "Code incorrect");
                                                }
                                            }
                                            else
                                            {
                                                Checker.SendResponseToClient(Checker.LastClient, "Code format incorrect");
                                            }
                                        }
                                    });
                                }
                            }
                        }
                        catch (NullReferenceException)
                        {
                            StopChecker();
                        }
                    }
                    Thread.Sleep(100); // ralentir la boucle pour éviter la surcharge
                }
            })
            {
                IsBackground = true
            };
            updateThread.Start();
        }

        private void StopChecker()
        {
            CheckerStarted = false;

            // Attendre la fin du thread avant de continuer
            updateThread?.Join();
        }


        private void ShowPopUp()
        {
            string message = "Pop-up affichée pour le groupe : ";
            foreach(CheckIn s in CheckInList)
            {
                message += s.student.Prenom + " " + s.student.Nom + " " + s.student.Code + " / ";
            }
            // Code pour afficher la pop-up ici
            MessageBox.Show(message);
        }
    }
}
