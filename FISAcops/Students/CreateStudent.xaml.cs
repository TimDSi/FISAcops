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
using System.Xml;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class CreateStudent : Page
    {
        private void SaveStudent_Click(object sender, RoutedEventArgs e)
        {
            string appLocation = @"D:\Projets\VisualStudio\FISAcops";
            string filePath = System.IO.Path.Combine(appLocation, "FISAcops", "Students","students.json");
            List<Student> students = new List<Student>();

            // Charger les étudiants existants à partir du fichier JSON
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                students = JsonSerializer.Deserialize<List<Student>>(json);
            }

            // Ajouter un nouvel étudiant à la liste
            students.Add(new Student
            {
                Nom = nomTextBox.Text,
                Prenom = prenomTextBox.Text,
                Mail = mailTextBox.Text,
                Promotion = promoTextBox.Text
            });

            // Sauvegarder la liste complète d'étudiants dans le fichier JSON
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true // Ajouter cette option pour formater le JSON de manière plus lisible
            };
            string output = JsonSerializer.Serialize(students, options);
            File.WriteAllText(filePath, output, new UTF8Encoding(false));
        }


        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        public CreateStudent()
        {
            InitializeComponent();
            Title = "Créer un élève"; // Ajoutez cette ligne pour modifier le titre de la fenêtre
        }
    }
}
