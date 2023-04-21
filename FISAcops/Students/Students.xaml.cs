using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Students : Page
    {
        private static List<Student> students = new List<Student>();
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'élève sélectionné
            var selectedStudentTag = ((Button)sender).Tag;
            var mainWindow = (MainWindow)Window.GetWindow(this);
            int index = -1;

            // Rechercher l'indice de l'étudiant dans la liste
            if (selectedStudentTag != null)
            {
                Student selectedStudent = (Student)selectedStudentTag;
                index = students.IndexOf(selectedStudent);
            }

            if (index >= 0)
            {
                // Naviguer vers la page d'édition de l'étudiant
                mainWindow.frame.Navigate(new EditStudent(index));
            }
            else
            {
                // L'étudiant sélectionné n'a pas été trouvé dans la liste
                // Gérer cette situation en conséquence
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'élève sélectionné
            var selectedStudent = ((Button)sender).Tag as Student;

            // Demander confirmation à l'utilisateur avant de supprimer l'élève
            var result = MessageBox.Show($"Voulez-vous vraiment supprimer l'élève {selectedStudent.Nom} ?", "Confirmation de suppression", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // Supprimer l'élève de la liste des étudiants
                var students = studentsListView.ItemsSource as List<Student>;
                students.Remove(selectedStudent);

                // Rafraîchir la liste des étudiants
                studentsListView.ItemsSource = null;
                studentsListView.ItemsSource = students;

                // Enregistrer les modifications dans le fichier JSON
                SaveStudentsToJson(students);
            }
        }

        private void SaveStudentsToJson(List<Student> students)
        {
            // Convertir la liste des étudiants en JSON
            string json = JsonSerializer.Serialize(students);

            // Enregistrer le JSON dans le fichier "students.json"
            string appLocation = @"D:\Projets\VisualStudio\FISAcops";
            string filePath = System.IO.Path.Combine(appLocation, "FISAcops", "Students", "students.json");
            File.WriteAllText(filePath, json);
        }

        public Students()
        {
            InitializeComponent();
            

            // Charger les étudiants à partir du fichier JSON
            string appLocation = @"D:\Projets\VisualStudio\FISAcops";
            string filePath = System.IO.Path.Combine(appLocation, "FISAcops", "Students", "students.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                students = JsonSerializer.Deserialize<List<Student>>(json);
            }

            // Lier la liste d'étudiants à notre ListView
            studentsListView.ItemsSource = students;
            Title = "Liste des élèves"; // Ajoutez cette ligne pour modifier le titre de la fenêtre

        }
    }

}
