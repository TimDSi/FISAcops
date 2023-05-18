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

        private List<CheckIn> StudentsWithCode = new();
        private List<int> codes = new();


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
            while (codes.Contains(newCode))
            {
                newCode = new Random().Next(1, 100001);
            }
            codes.Add(newCode);
            return newCode;
        }

        private void DeleteStudentFromDetection(StudentWithCode student)
        {
            int i = 0;
            while ( i < StudentsWithCode.Count)
            {
                if (StudentsWithCode[i].student.Mail == student.Mail)
                {
                    break;
                }
                i++;
            }
            codes.RemoveAt(i);
            StudentsWithCode.RemoveAt(i);
        }

        private void DetectionLoop()
        {
            while (!stopDetection)
            {
                // Obtenir les appels pour la date actuelle
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                List<Call> calls = CallsService.LoadCallsForSelectedDate(currentDate);

                // Vérifier si la pop-up doit être affichée pour chaque appel
                foreach (Call call in calls)
                {
                    DateTime callDateTime = ParseDateTime(call.Date, call.Time);
                    DateTime currentDateTime = DateTime.Now;

                    DateTime callDateTimePlusOneMinute = callDateTime.AddMinutes(1);


                    if (currentDateTime >= callDateTime && currentDateTime < callDateTimePlusOneMinute)
                    {
                        foreach(StudentWithState student in call.StudentsWithState)
                        {
                            if(student.State == "Controle")
                            {
                                StudentsWithCode.Add(new ((StudentWithCode)StudentFactory.CreateStudent(
                                    student.Nom, 
                                    student.Prenom, 
                                    student.Mail, 
                                    student.Promotion, 
                                    GenerateCode().ToString()
                                )));
                            }
                        }

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

    }
}
