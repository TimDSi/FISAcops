﻿using FISAcops.CheckIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class CallTest : Page
    {
        private readonly List<Group> groupsList;
        private List<IStudent> studentsListWithCode;


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

        private List<CheckIn> checkIns = new();

        private static List<CheckIn> GenerateCheckIns(List<Student> students)
        {
            List<CheckIn> checkIns = new();
            List<int> codes = new();
            int groupSize = students.Count;

            while (codes.Count < groupSize)
            {
                int newCode = new Random().Next(1, 99999);
                if (!codes.Contains(newCode))
                {
                    codes.Add(newCode);
                }
            }

            for (int i = 0; i < groupSize; i++)
            {
                checkIns.Add(new CheckIn((StudentWithCode)StudentFactory.CreateStudent(
                    students[i].Nom,
                    students[i].Prenom,
                    students[i].Mail,
                    students[i].Promotion, 
                    codes[i])
                    ));
            }

            return checkIns;
        }

        // serveur sur un thread -------------------------------------------------------------------
        private bool CheckerStarted = false;
        Thread? updateThread;

        private void StartChecker()
        {
            CheckerStarted = true;
            updateThread = new(() =>
            {
                while (CheckerStarted)
                {
                    string receivedMessage = Checker.ReceivedMessage;
                    if (!string.IsNullOrEmpty(receivedMessage))
                    {
                        try
                        {
                            if (Application.Current != null)
                            {
                                if (!Application.Current.Dispatcher.HasShutdownStarted)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
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

                                                        int index = studentsListWithCode.FindIndex(s => s.Mail == checkIn.student.Mail);
                                                        if (index != -1)
                                                        {
                                                            IStudent s = studentsListWithCode[index];
                                                            studentsListWithCode[index] = StudentFactory.CreateStudent(s.Nom, s.Prenom, s.Mail, s.Promotion, "Code bon");
                                                        }
                                                        dgStudents.Items.Refresh(); // Rafraîchir uniquement les éléments du DataGrid


                                                        Checker.SendResponseToClient(checkIn.CodeMessage(enteredCode));
                                                        noCode = false;
                                                    }
                                                }
                                                if (noCode)
                                                {
                                                    Checker.SendResponseToClient("Code incorrect");
                                                }
                                            }
                                            else
                                            {
                                                Checker.SendResponseToClient("Code format incorrect");
                                            }
                                        }
                                    });
                                }
                            }
                        }
                        catch (NullReferenceException)
                        {
                            StopChecker();
                        }
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
            CheckerStarted = false;

            // Attendre la fin du thread avant de continuer
            updateThread?.Join();

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
                studentsListWithCode.Add(StudentFactory.CreateStudent(student.Nom, student.Prenom, student.Mail, student.Promotion, checkIn.GetCode()));
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


        public CallTest()
        {
            cbStudents = new ComboBox();
            cbGroups = new ComboBox();

            InitializeComponent();
            studentsListWithCode = new();

            cbStudents.ItemsSource = StudentsService.LoadStudentsFromJson();
            cbStudents.SelectedIndex = 0;

            groupsList = GroupsService.LoadGroupsFromJson();
            foreach (var group in groupsList)
            {
                cbGroups.Items.Add(group.GroupName);
            }
            cbGroups.SelectedIndex = 0;
        }
    }
}
