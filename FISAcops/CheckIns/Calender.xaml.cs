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


        public Calender()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = ((Calendar)sender).SelectedDate.GetValueOrDefault();
            SelectedDateText = selectedDate.ToShortDateString();
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


        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }
    }
}
