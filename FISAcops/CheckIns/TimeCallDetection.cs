using FISAcops.CheckIns;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace FISAcops
{
    public class TimeCallDetection
    {
        private readonly Thread detectionThread;
        private bool stopDetection;

        private List<CheckIn> CheckInList = new();
        private List<int> Codes = new();
        private List<DateTime> DeleteTime = new();

        List<Result> resultList = new();


        public TimeCallDetection()
        {
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
        }

        

        private void DetectionLoop()
        {

            while (!stopDetection)
            {
                // Obtenir les appels pour la date actuelle
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                List<Call> calls = CallsService.LoadCallsForSelectedDate(currentDate);

                DateTime currentDateTime = DateTime.Now;


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
                        }
                    }

                    for (int i = resultList.Count - 1; i >= 0; i--)
                    {
                        Result result = resultList[i];
                        if (result.IsResultDone())
                        {
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
                                    GenerateCode().ToString()
                                );
                                CheckInList.Add(new CheckIn(studentWithCode));
                                Codes.Add(int.Parse(studentWithCode.Code));
                                DeleteTime.Add(callDateTime.AddMinutes(2));
                            }
                        }
                        ShowPopUp();
                    }
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
