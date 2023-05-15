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
        private readonly HashSet<string> groupsWithPopUpShown = new();

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
                    if (!groupsWithPopUpShown.Contains(call.GroupName))
                    {
                        DateTime callDateTime = ParseDateTime(call.Date, call.Time);
                        DateTime currentDateTime = DateTime.Now;

                        if (currentDateTime >= callDateTime)
                        {
                            // Afficher la pop-up pour le groupe
                            ShowPopUp(call.GroupName);

                            // Ajouter le groupe à l'ensemble des groupes ayant la pop-up affichée
                            groupsWithPopUpShown.Add(call.GroupName);
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


        private static void ShowPopUp(string groupName)
        {
            // Code pour afficher la pop-up ici
            MessageBox.Show($"Pop-up affichée pour le groupe {groupName}");
        }
    }
}
