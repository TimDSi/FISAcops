using FISAcops.CheckIns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
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
        //private readonly string groupPath = System.IO.Path.Combine(new Settings().groupsPath, "groups.json");
        private readonly List<Group> groupsList;
        
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


        private static List<Group> LoadGroupsFromJson()
        {
            var groupPath = System.IO.Path.Combine(new Settings().groupsPath, "groups.json");
            if (!File.Exists(groupPath))
            {
                return new List<Group>();
            }
            var json = File.ReadAllText(groupPath);
            return JsonSerializer.Deserialize<List<Group>>(json);
        }

        private static List<Student> LoadStudentsFromJson()
        {
            var studentsPath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");
            if (!File.Exists(studentsPath))
            {
                return new List<Student>();
            }
            var json = File.ReadAllText(studentsPath);
            return JsonSerializer.Deserialize<List<Student>>(json);
        }

        private List<CheckIn> checkIns = new();

        private static List<CheckIn> GenerateCheckIns(List<Student> students)
        {
            List<CheckIn> checkIns = new();
            List<int> codes = new();
            int groupSize = students.Count;

            while (codes.Count < groupSize)
            {
                int newCode = new Random().Next(1, 100001);
                if (!codes.Contains(newCode))
                {
                    codes.Add(newCode);
                }
            }

            for (int i = 0; i < groupSize; i++)
            {
                checkIns.Add(new CheckIn($"{students[i].Nom} {students[i].Prenom}", codes[i]));
            }

            return checkIns;
        }

        private bool CheckerStarted = false;

        private void StartChecker()
        {
            CheckerStarted = true;
            Thread checkerThread = new(CheckerThreadMethod)
            {
                IsBackground = true
            };
            checkerThread.Start();
        }

        private void StopChecker()
        {
            CheckerStarted = false;
        }

        private void CheckerThreadMethod()
        {
            Checker checker = new();
            while (CheckerStarted && checker.ReceivedMessage == "")
            {
                Dispatcher.Invoke(() => txtCode.Text = checker.ReceivedMessage);
            }
            Dispatcher.Invoke(() => txtCode.Text = checker.ReceivedMessage);
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            tbRandomNumber.Text = "";
            checkIns.Clear();

            if (rbStudents.IsChecked == true && cbStudents.SelectedItem is Student selectedStudent)
            {
                checkIns = GenerateCheckIns(new List<Student> { selectedStudent });
                tbRandomNumber.Text = checkIns[^1].GetCode().ToString();
            }
            else if (rbGroups.IsChecked == true && cbGroups.SelectedItem is string groupName)
            {
                Group? selectedGroup = groupsList.FirstOrDefault(g => g.GroupName == groupName);
                if (selectedGroup != null)
                {
                    checkIns = GenerateCheckIns(selectedGroup.StudentsList);
                    foreach (var checkIn in checkIns)
                    {
                        tbRandomNumber.Text += " " + checkIn.GetCode().ToString();
                    }
                }
            }
            tbState.Text = "Code non rentré";
            StartChecker();
        }


        private void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(txtCode.Text, out int enteredCode))
            {
                bool noCode = true;
                foreach (CheckIn checkIn in checkIns)
                {
                    if (checkIn.IsCodeGood(enteredCode))
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
            StopChecker();
        }


        public Call()
        {
            cbStudents = new ComboBox();
            cbGroups = new ComboBox();

            InitializeComponent();


            cbStudents.ItemsSource = LoadStudentsFromJson();
            cbStudents.SelectedIndex = 0;

            groupsList = LoadGroupsFromJson();
            foreach (var group in groupsList)
            {
                cbGroups.Items.Add(group.GroupName);
            }
            cbGroups.SelectedIndex = 0;

        }
    }
}
