using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GroupEdition : Page
    {

        private readonly string filePath = System.IO.Path.Combine(new Settings().studentsPath, "groups.json");

        private List<Student> AvailableStudents = new();
        private List<Student> SelectedStudents = new();

        private List<Group> groupsList = new();
        private readonly int? selectedGroup;

        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        private void SaveGroup_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGroup == null)
            {
                // Ajouter un nouveau groupe à la liste
                groupsList.Add(new Group(nomTextBox.Text, new List<Student>()));
            }
            else
            {
                // Modifier le groupe sélectionné dans la liste
                //selectedGroup.GroupName = nomTextBox.Text;
            }


            // Sauvegarder la liste complète d'étudiants dans le fichier JSON
            var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true // Ajouter cette option pour formater le JSON de manière plus lisible
                };
            string output = JsonSerializer.Serialize(groupsList, options);
            File.WriteAllText(filePath, output, new UTF8Encoding(false));

            ReturnAndLoad();
        }

        private void ReturnAndLoad()
        {
            // Récupérer la fenêtre courante
            var mainWindow = (MainWindow)Window.GetWindow(this);

            // Navigate to Students
            mainWindow.frame.Navigate(new Groups());
        }

        private void RefreshLists()
        {
            /*
            // Vider les deux listes
            AvailableStudents.Clear();
            SelectedStudents.Clear();

            // Ajouter les élèves disponibles dans la première liste
            foreach (Student student in AvailableStudents)
            {
                AvailableStudents.Add(student.Nom + " " + student.Prenom);
            }

            // Ajouter les élèves sélectionnés dans la seconde liste
            foreach (Student student in SelectedStudents)
            {
                SelectedStudents.Add(student.Nom + " " + student.Prenom);
            }*/
        }


        public GroupEdition()
        {
            InitializeComponent();

            string filePath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");
            string groupPath = System.IO.Path.Combine(new Settings().groupsPath, "students.json");

            // Charger les étudiants à partir du fichier JSON
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                AvailableStudents = JsonSerializer.Deserialize<List<Student>>(json);
                SelectedStudents = JsonSerializer.Deserialize<List<Student>>(json);
            }

            if (File.Exists(groupPath))
            {
                string json = File.ReadAllText(groupPath);
                groupsList = JsonSerializer.Deserialize<List<Group>>(json);
            }
            
        }

    }
}
