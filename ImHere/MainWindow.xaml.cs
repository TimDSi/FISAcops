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
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
