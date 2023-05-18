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


                    if (currentDateTime >= callDateTime && currentDateTime < callDateTimePlusOneMinute)
                    {
                        foreach(StudentWithState student in call.StudentsWithState)
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
