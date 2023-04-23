using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private readonly string groupPath = System.IO.Path.Combine(new Settings().studentsPath, "groups.json");

        private List<Student> AvailableStudents = new();
        private List<Student> SelectedStudents = new();

        private List<Group> groupsList = new();
        private int index;
        private readonly int? selectedGroup;

        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new Groups());
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Student student = checkBox.DataContext as Student;
            SelectedStudents.Add(student);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Student student = checkBox.DataContext as Student;
            SelectedStudents.Remove(student);
        }


        private void SaveGroup_Click(object sender, RoutedEventArgs e)
        {

            // Vérifier si le nom du groupe est déjà utilisé
            bool groupNameExists = groupsList.Any(group => group.GroupName == nomTextBox.Text);


            if (selectedGroup == null)
            {
                if (groupNameExists)
                {
                    // Afficher un message d'erreur si un groupe avec le même nom existe déjà
                    MessageBox.Show("Un groupe avec le même nom existe déjà. Veuillez choisir un nom différent.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Ajouter un nouveau groupe à la liste
                groupsList.Add(new Group(nomTextBox.Text, SelectedStudents));
            }
            else
            {
                if (groupNameExists && groupsList[selectedGroup.Value].GroupName != nomTextBox.Text)
                {
                    // Afficher un message d'erreur si un groupe avec le même nom existe déjà et que ce n'est pas le groupe actuellement sélectionné
                    MessageBox.Show("Un groupe avec le même nom existe déjà. Veuillez choisir un nom différent.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Modifier le groupe sélectionné dans la liste
                groupsList[selectedGroup.Value].GroupName = nomTextBox.Text;
                groupsList[selectedGroup.Value].StudentsList = SelectedStudents;
            }


            // Sauvegarder la liste complète d'étudiants dans le fichier JSON
            var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true // Ajouter cette option pour formater le JSON de manière plus lisible
                };
            string output = JsonSerializer.Serialize(groupsList, options);
            File.WriteAllText(groupPath, output, new UTF8Encoding(false));

            ReturnAndLoad();
        }

        private void ReturnAndLoad()
        {
            // Récupérer la fenêtre courante
            var mainWindow = (MainWindow)Window.GetWindow(this);

            // Navigate to Students
            mainWindow.frame.Navigate(new Groups());
        }


        public GroupEdition()
        {
            InitializeComponent();

            string studentsPath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");

            // Charger les étudiants à partir du fichier JSON
            if (File.Exists(studentsPath))
            {
                string json = File.ReadAllText(studentsPath);
                AvailableStudents = JsonSerializer.Deserialize<List<Student>>(json);
                SelectedStudents = JsonSerializer.Deserialize<List<Student>>(json);
            }

            if (File.Exists(groupPath))
            {
                string json = File.ReadAllText(groupPath);
                groupsList = JsonSerializer.Deserialize<List<Group>>(json);
            }

            dgStudents2.ItemsSource = AvailableStudents;
            dgStudents1.Visibility = Visibility.Collapsed;

        }

        public GroupEdition(int selectedGroup)
        {
            InitializeComponent();
            this.selectedGroup = selectedGroup;
            string studentsPath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");

            // Charger les étudiants à partir du fichier JSON
            if (File.Exists(studentsPath))
            {
                string json = File.ReadAllText(studentsPath);
                AvailableStudents = JsonSerializer.Deserialize<List<Student>>(json);
            }

            if (File.Exists(groupPath))
            {
                string json = File.ReadAllText(groupPath);
                groupsList = JsonSerializer.Deserialize<List<Group>>(json);
                SelectedStudents = groupsList[selectedGroup].StudentsList;
                nomTextBox.Text = groupsList[selectedGroup].GroupName;

                // Supprimer les étudiants sélectionnés de la liste AvailableStudents
                AvailableStudents.RemoveAll(student => SelectedStudents.Any(s => s.Mail == student.Mail));
            }


            dgStudents2.ItemsSource = AvailableStudents;
            dgStudents1.ItemsSource = SelectedStudents;
        }
    }
}
