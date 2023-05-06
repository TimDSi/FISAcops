using FISAcops.CheckIns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


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
            StopChecker();
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

        // serveur sur un thread -------------------------------------------------------------------
        private bool CheckerStarted = false;
        private readonly Checker checker = new();

        private void StartChecker()
        {
            CheckerStarted = true;
            Thread checkerThread = new(checker.CheckerStart)
            {
                IsBackground = true
            };
            checkerThread.Start();

            Thread updateThread = new(() =>
            {
                while (CheckerStarted)
                {
                    string receivedMessage = checker.ReceivedMessage;
                    if (!string.IsNullOrEmpty(receivedMessage))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            txtCode.Text = receivedMessage;
                            if (Int32.TryParse(txtCode.Text, out int enteredCode))
                            {
                                bool noCode = true;
                                foreach (CheckIn checkIn in checkIns)
                                {
                                    if (checkIn.IsCodeGood(enteredCode))
                                    {
                                        tbState.Text = checkIn.CodeMessage(enteredCode);
                                        if (Checker.LastClient != null)
                                        {
                                            Checker.SendResponseToClient(Checker.LastClient, tbState.Text);
                                        }
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
                        });

                        
                    }
                    Thread.Sleep(100); // ralentir la boucle pour éviter la surcharge
                }
            })
            {
                IsBackground = true
            };
            updateThread.Start();
        }

        private void StopChecker()
        {
            checker.CheckerStop();
            CheckerStarted = false;
            
        }

        //------------------------------------------------------------------------------------------


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
            //tbState.Text = "Code non rentré";
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
