using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Calender : Page, INotifyPropertyChanged
    {
        private string? _selectedDateText;
        public string? SelectedDateText
        {
            get { return _selectedDateText; }
            set
            {
                _selectedDateText = value;
                OnPropertyChanged(nameof(SelectedDateText));
            }
        }

        public List<Call>? Calls;
        public List<Call>? FilteredCalls { get; set; }


        public Calender()
        {
            InitializeComponent();
            DataContext = this;

            // Initialiser la date à la date d'aujourd'hui
            DateTime today = DateTime.Today;
            SelectedDateText = today.ToShortDateString();

            // Filtrer les appels en fonction de la date d'aujourd'hui
            FilteredCalls = CallsService.LoadCallsForSelectedDate(SelectedDateText);

            // Mettre à jour la liste des appels dans le DataGrid
            CallsByDate.ItemsSource = FilteredCalls;
            OnPropertyChanged(nameof(FilteredCalls));
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = ((Calendar)sender).SelectedDate.GetValueOrDefault();
            SelectedDateText = selectedDate.ToShortDateString();

            // Filtrer les appels en fonction de la date sélectionnée
            FilteredCalls = CallsService.LoadCallsForSelectedDate(SelectedDateText);

            // Mettre à jour la liste des appels dans le DataGrid
            CallsByDate.ItemsSource = FilteredCalls;
            OnPropertyChanged(nameof(FilteredCalls));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EditCallButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);

            string? selectedDate = SelectedDateText; // Obtenir la valeur de la TextBox

            if (selectedDate != null) {
                // Naviguer vers la page "EditCall" en passant la date sélectionnée en tant que paramètre
                mainWindow.frame.Navigate(new EditCall(new Call(selectedDate,"08:30","","once")));
            }
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir le bouton qui a déclenché l'événement
            var modifierButton = (Button)sender;

            // Obtenir l'objet Call associé à la ligne du bouton cliqué
            var call = (Call)modifierButton.DataContext;

            // Effectuer les actions de modification avec l'objet Call
            // ...

            // Exemple : Naviguer vers la page "EditCall" en passant l'objet Call en tant que paramètre
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new EditCall(call));
        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir le bouton qui a déclenché l'événement
            var supprimerButton = (Button)sender;

            // Obtenir l'objet Call associé à la ligne du bouton cliqué
            var call = (Call)supprimerButton.DataContext;

            // Charger la liste des appels depuis le fichier JSON
            List<Call> calls = CallsService.LoadCallsFromJson();

            // Recherche de l'index de l'appel dans la liste
            var index = -1;
            for (var i = 0; i < calls.Count; i++)
            {
                var callFromList = calls[i];
                if (call.Date == callFromList.Date
                    && call.Time == callFromList.Time
                    && call.GroupName == callFromList.GroupName
                    && call.Frequency == callFromList.Frequency)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                // Supprimer l'appel de la liste
                calls.RemoveAt(index);

                // Sauvegarder la liste mise à jour dans le fichier JSON
                CallsService.SaveCallsToJson(calls);

                // Supprimer l'appel de la liste filtrée
                FilteredCalls?.Remove(call);

                // Mettre à jour la liste des appels dans le DataGrid
                CallsByDate.ItemsSource = null;
                CallsByDate.ItemsSource = FilteredCalls;
            }
        }



        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }
    }
}
