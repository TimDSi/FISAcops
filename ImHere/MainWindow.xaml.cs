using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace ImHere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            int enteredCode;
            if (Int32.TryParse(tbCode.Text, out enteredCode))
            {
                tbState.Text = "Code enregistré : " + enteredCode;
            }
            else
            {
                tbState.Text = "Code incorrect";
            }

            try
            {
                // Création d'un client de socket
                TcpClient client = new TcpClient();

                // Connexion au serveur distant
                client.Connect("127.0.0.1", 8080);

                // Envoi de données au serveur
                byte[] data = System.Text.Encoding.ASCII.GetBytes(enteredCode.ToString());
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                // Fermeture du client de socket
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
