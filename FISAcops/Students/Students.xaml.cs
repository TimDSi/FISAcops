using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Students : Page
    {

        private string filePath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");

        private static List<Student> StudentsList = new();
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

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

            if (index >= 0)
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
                    var groups = LoadGroupsFromJson();
                    foreach (var group in groups)
                    {
                        var studentToRemove = group.StudentsList.FirstOrDefault(s => s.Mail == selectedStudent.Mail);
                        if (studentToRemove != null)
                        {
                            group.StudentsList.Remove(studentToRemove);
                        }
                    }
                    SaveGroupsToJson(groups);


                    // Supprimer l'élève de la liste des étudiants
                    students.Remove(selectedStudent);

                    // Rafraîchir la liste des étudiants
                    studentsListView.ItemsSource = null;
                    studentsListView.ItemsSource = students;

                    // Enregistrer les modifications dans le fichier JSON
                    SaveStudentsToJson(students);
                }
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

        private void SaveGroupsToJson(List<Group> groups)
        {
            var groupPath = System.IO.Path.Combine(new Settings().groupsPath, "groups.json");
            var json = JsonSerializer.Serialize(groups, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(groupPath, json);
        }


        public void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new StudentEdition());
        }

        private void SaveStudentsToJson(List<Student> students)
        {
            var studentsPath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");
            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(studentsPath, json);
        }

        public void RefreshStudentsList()
        {
            // Charger les étudiants à partir du fichier JSON
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                StudentsList = JsonSerializer.Deserialize<List<Student>>(json);
            }

            // Rafraîchir la source de données de la ListView
            studentsListView.ItemsSource = null;
            studentsListView.ItemsSource = StudentsList;
        }

        public Students()
        {
            InitializeComponent();
            RefreshStudentsList();
            // Lier la liste d'étudiants à notre ListView
            studentsListView.ItemsSource = StudentsList;
            Title = "Liste des élèves"; // Ajoutez cette ligne pour modifier le titre de la fenêtre

        }
    }

}
