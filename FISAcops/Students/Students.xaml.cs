﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Students : Page
    {
        private StudentsPageMemento? currentMemento;


        private static List<Student> StudentsList = new();
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        private bool filtered = false;

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'élève sélectionné
            var selectedStudentTag = ((Button)sender).Tag;
            int index = -1;

            // Rechercher l'indice de l'étudiant dans la liste
            if (selectedStudentTag != null)
            {
                Student selectedStudent = (Student)selectedStudentTag;
                index = StudentsList.IndexOf(selectedStudent);
            }

            if (index != -1)
            {
                // Naviguer vers la page d'édition de l'étudiant
                var mainWindow = (MainWindow)Window.GetWindow(this);
                mainWindow.frame.Navigate(new StudentEdition(index));
            }
            else
            {
                // L'étudiant sélectionné n'a pas été trouvé dans la liste
                // Gérer cette situation en conséquence
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            // retrive student list and selected student
            if (
                ((Button)sender).Tag is Student selectedStudent && 
                studentsListView.ItemsSource is List<Student> students
                )
            {
                // Demander confirmation à l'utilisateur avant de supprimer l'élève
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer l'élève {selectedStudent.Nom} ?", "Confirmation de suppression", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Supprimer l'étudiant de tous les groupes qui le contiennent
                    var groups = GroupsService.LoadGroupsFromJson();
                    foreach (var group in groups)
                    {
                        var studentToRemove = group.StudentsList.FirstOrDefault(s => s.Mail == selectedStudent.Mail);
                        if (studentToRemove != null)
                        {
                            group.StudentsList.Remove(studentToRemove);
                        }
                    }
                    GroupsService.SaveGroupsToJson(groups);


                    // Supprimer l'élève de la liste des étudiants
                    students.Remove(selectedStudent);

                    // Rafraîchir la liste des étudiants
                    studentsListView.ItemsSource = null;
                    studentsListView.ItemsSource = students;

                    // Enregistrer les modifications dans le fichier JSON
                    StudentsService.SaveStudentsToJson(students);
                }
            }

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string promotion = searchTextBox.Text;

            if (promotion == "")
            {
                filtered = false;
                UndoSearch();
            }
            else if (!filtered)
            {
                // Créer un nouveau Memento avant la recherche
                currentMemento = new StudentsPageMemento(StudentsList);
                filtered = true;
            }

            if (filtered)
            {
                StudentsList = StudentsList.Where(student => student.Promotion.Contains(promotion)).ToList();
                studentsListView.ItemsSource = StudentsList;
            }
        }

        private void RestoreFromMemento(StudentsPageMemento memento)
        {
            StudentsList = new List<Student>(memento.Students);
            studentsListView.ItemsSource = StudentsList;
        }

        private void UndoSearch()
        {
            if (currentMemento != null)
            {
                RestoreFromMemento(currentMemento);
            }
        }


        public void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Annuler la recherche avant d'ajouter un nouvel étudiant
            UndoSearch();

            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new StudentEdition());
        }

        public Students()
        {
            InitializeComponent();
            StudentsList = StudentsService.LoadStudentsFromJson();

            // Lier la liste d'étudiants à notre ListView
            studentsListView.ItemsSource = StudentsList;
            Title = "Liste des élèves"; // Ajoutez cette ligne pour modifier le titre de la fenêtre

            // Ajouter un gestionnaire d'événement pour l'événement TextChanged du TextBox
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
        }
    }

}
