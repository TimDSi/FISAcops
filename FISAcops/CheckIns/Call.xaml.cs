﻿using FISAcops.CheckIns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Call : Page
    {
        //private readonly string groupPath = System.IO.Path.Combine(new Settings().groupsPath, "groups.json");
        private readonly List<Group> groupsList;
        private List<Student> studentsListWithCode;


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
                            if (Checker.LastClient != null)
                            {
                                if (int.TryParse(receivedMessage, out int enteredCode))
                                {
                                    bool noCode = true;
                                    foreach (CheckIn checkIn in checkIns)
                                    {
                                        if (checkIn.IsCodeGood(enteredCode))
                                        {
                                            int index = studentsListWithCode.FindIndex(s => s.Mail == checkIn.Mail);
                                            //dgStudents.Items[index] = StudentFactory.CreateStudent(studentsListWithCode[index].Nom, studentsListWithCode[index].Prenom, studentsListWithCode[index].Mail, studentsListWithCode[index].Promotion, "Code bon");
                                            Checker.SendResponseToClient(Checker.LastClient, checkIn.CodeMessage(enteredCode));
                                            noCode = false;
                                            dgStudents.Items.Refresh();
                                        }
                                    }
                                    if (noCode)
                                    {
                                        Checker.SendResponseToClient(Checker.LastClient, "Code incorrect");
                                    }
                                }
                                else
                                {
                                    Checker.SendResponseToClient(Checker.LastClient, "Code format incorrect");
                                }
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

            // Réactiver les RadioButton
            rbStudents.IsEnabled = true;
            rbGroups.IsEnabled = true;

            // Réactiver les ComboBox
            cbStudents.IsEnabled = true;
            cbGroups.IsEnabled = true;
        }

        //------------------------------------------------------------------------------------------


        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            checkIns.Clear();
            List<Student> studentsList = new();
            if (rbStudents.IsChecked == true && cbStudents.SelectedItem is Student selectedStudent)
            {
                studentsList.Add(selectedStudent);
            }
            else if (rbGroups.IsChecked == true && cbGroups.SelectedItem is string groupName)
            {
                Group? selectedGroup = groupsList.FirstOrDefault(g => g.GroupName == groupName);
                if (selectedGroup != null)
                {
                    studentsList = selectedGroup.StudentsList;
                }
            }
            checkIns = GenerateCheckIns(studentsList);

            studentsListWithCode = new();
            for (int i = 0; i < studentsList.Count; i++)
            {
                Student student = studentsList[i];
                CheckIn checkIn = checkIns[i];
                studentsListWithCode.Add(StudentFactory.CreateStudent(student.Nom, student.Prenom, student.Mail, student.Promotion, checkIn.GetCode().ToString()));
            }
            StartChecker();

            

            // Définition de la source de données du DataGrid
            dgStudents.ItemsSource = studentsListWithCode;

            btnReactivate.IsEnabled = true;

            // Désactiver les RadioButton
            rbStudents.IsEnabled = false;
            rbGroups.IsEnabled = false;

            // Désactiver les ComboBox
            cbStudents.IsEnabled = false;
            cbGroups.IsEnabled = false;
        }


        private void BtnReactivate_Click(object sender, RoutedEventArgs e)
        {
            StopChecker();
        }


        public Call()
        {
            cbStudents = new ComboBox();
            cbGroups = new ComboBox();

            InitializeComponent();
            studentsListWithCode = new();

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
