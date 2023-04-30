using FISAcops.CheckIns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
    public partial class Call : Page
    {
        
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (rbStudents.IsChecked == true)
            {
                cbStudents.Visibility = Visibility.Visible;
                cbGroups.Visibility = Visibility.Collapsed;
            }
            else if (rbGroups.IsChecked == true)
            {
                cbStudents.Visibility = Visibility.Collapsed;
                cbGroups.Visibility = Visibility.Visible;

                
            }
        }


        private List<Group> LoadGroupsFromJson()
        {
            var groupPath = System.IO.Path.Combine(new Settings().groupsPath, "groups.json");
            if (!File.Exists(groupPath))
            {
                return new List<Group>();
            }
            var json = File.ReadAllText(groupPath);
            return JsonSerializer.Deserialize<List<Group>>(json);
        }

        private List<Student> LoadStudentsFromJson()
        {
            var studentsPath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");
            if (!File.Exists(studentsPath))
            {
                return new List<Student>();
            }
            var json = File.ReadAllText(studentsPath);
            return JsonSerializer.Deserialize<List<Student>>(json);
        }

        private List<CheckIn> checkIns = new List<CheckIn>();

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            tbRandomNumber.Text = "";
            checkIns.Clear();
            if (rbStudents.IsChecked == true)
            {
                checkIns.Add(new CheckIn("TestSolo" , new Random().Next(1, 100001)));
                tbRandomNumber.Text = checkIns[checkIns.Count - 1].getCode().ToString();
            }
            else if (rbGroups.IsChecked == true)
            {
                int tailleGroupe = 3;
                List<int> codes = new List<int>();
                while (codes.Count < tailleGroupe)
                {
                    int newCode = new Random().Next(1, 100001);
                    if (!codes.Contains(newCode))
                    {
                        codes.Add(newCode);
                    }
                }
                for (int i = 0; i < tailleGroupe; i++)
                {
                    checkIns.Add(new CheckIn("TestMulti"+i, codes[i]));
                    tbRandomNumber.Text = tbRandomNumber.Text + " " + checkIns[checkIns.Count - 1].getCode().ToString();
                }
            }
            tbState.Text = "Code non rentré";
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            int enteredCode;
            if (Int32.TryParse(txtCode.Text, out enteredCode))
            {
                bool noCode = true;
                foreach (CheckIn checkIn in checkIns)
                {
                    if (checkIn.isCodeGood(enteredCode))
                    {
                        tbState.Text = checkIn.CodeMessage(enteredCode);
                        noCode = false;
                    }
                }
                if (noCode)
                {
                    tbState.Text = "Code incorrect";
                }
            }
            else
            {
                tbState.Text = "Code incorrect";
            }
        }

        public Call()
        {
            cbStudents = new ComboBox();
            cbGroups = new ComboBox();

            InitializeComponent();
            
            cbStudents.ItemsSource = LoadStudentsFromJson();
            cbStudents.SelectedIndex = 0;

            var groups = LoadGroupsFromJson();
            foreach (var group in groups)
            {
                cbGroups.Items.Add(group.GroupName);
            }
            cbGroups.SelectedIndex = 0;

        }
    }
}
