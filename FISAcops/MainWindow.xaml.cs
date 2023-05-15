using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private TimeCallDetection timeCallDetection;

        public MainWindow()
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new MainPage());
            // Définir la couleur de fond souhaitée
            var backgroundColor = Brushes.LightBlue;

            // Instancier la classe TimeCallDetection
            timeCallDetection = new TimeCallDetection();

            // Changer la couleur de fond de la fenêtre principale
            Background = backgroundColor;

            // Démarrer la détection des appels
            timeCallDetection.StartDetection();
        }
    }
}
