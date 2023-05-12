using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    public partial class EditCall : Page
    {
        public string Date { get; set; }
        public List<string> TimeSlots { get; set; }
        public string SelectedTimeSlot { get; set; }
        public bool IsDateReadOnly { get; set; }

        private readonly List<Group> groupsList;
        private readonly List<Call> callsList;
        private readonly int originalCallIndex = -1;

        public EditCall(Call call)
        {
            InitializeComponent();
            DataContext = this;

            Date = call.Date;
            IsDateReadOnly = true;

            TimeSlots = GenerateTimeSlots();
            SelectedTimeSlot = call.Time;

            groupsList = GroupsService.LoadGroupsFromJson();
            foreach (var group in groupsList)
            {
                cbGroups.Items.Add(group.GroupName);
            }
            cbGroups.SelectedIndex = 0;

            callsList = CallsService.LoadCallsFromJson();

            if (!string.IsNullOrEmpty(call.GroupName))
            {
                // Édition : trouver l'index du Call original dans la liste des appels
                for(var i = 0; i < callsList.Count ; i++)
                {
                    var callFromList = callsList[i];
                    if (call.Date == callFromList.Date
                        && call.Time == callFromList.Time
                        && call.GroupName == callFromList.GroupName
                        && call.Frequency == callFromList.Frequency)
                    {
                        originalCallIndex = i;
                        break;
                    }
                }
            }
        }

        private static List<string> GenerateTimeSlots()
        {
            List<string> timeSlots = new();
            TimeSpan startTime = new(8, 0, 0);
            TimeSpan endTime = new(18, 0, 0);
            TimeSpan timeSlotDuration = new(0, 15, 0);

            while (startTime <= endTime)
            {
                timeSlots.Add(startTime.ToString(@"hh\:mm"));
                startTime = startTime.Add(timeSlotDuration);
            }

            return timeSlots;
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            string date = Date;
            string selectedTimeSlot = (string)cbTimeSlots.SelectedItem;
            string selectedGroup = (string)cbGroups.SelectedItem;
            string? selectedFrequency = ((ComboBoxItem)cbFrequency.SelectedItem)?.Content.ToString();
            List<Call> callsList = CallsService.LoadCallsFromJson();
            selectedFrequency ??= "Once";

            if (originalCallIndex != -1)
            {
                // Mettre à jour le Call à l'index avec les nouvelles valeurs
                callsList[originalCallIndex] = new Call(date, selectedTimeSlot, selectedGroup, selectedFrequency);
            }
            else
            {
                // Ajout d'un nouveau Call à la liste
                callsList.Add(new Call(date, selectedTimeSlot, selectedGroup, selectedFrequency));
            }

            CallsService.SaveCallsToJson(callsList);
            NavigateToCalendar();
        }

        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            NavigateToCalendar();
        }

        private void NavigateToCalendar()
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new Calender());
        }
    }
}
