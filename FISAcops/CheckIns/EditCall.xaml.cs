using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for EditCall.xaml
    /// </summary>
    public partial class EditCall : Page
    {
        public string Date { get; set; } // Utilisez une propriété avec un getter et un setter
        public List<string> TimeSlots { get; set; }
        public string SelectedTimeSlot { get; set; }
        public bool IsDateReadOnly { get; set; }

        private readonly List<Group> groupsList;

        public EditCall(Call call)
        {
            InitializeComponent();
            this.DataContext = this;

            // Initialisez la valeur de la propriété Date
            this.Date = call.Date;
            // Définissez IsDateReadOnly sur true pour rendre la TextBox en lecture seule
            this.IsDateReadOnly = true;

            // Initialisez la liste des tranches horaires
            TimeSlots = GenerateTimeSlots();

            this.SelectedTimeSlot = call.Time;

            groupsList = GroupsService.LoadGroupsFromJson();
            foreach (var group in groupsList)
            {
                cbGroups.Items.Add(group.GroupName);
            }
            cbGroups.SelectedIndex = 0;
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
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new Calender());
        }
    }
}
