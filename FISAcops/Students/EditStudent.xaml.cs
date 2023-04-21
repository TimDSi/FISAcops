using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class EditStudent : Page
    {
        private static string appLocation = @"D:\Projets\VisualStudio\FISAcops";
        private static string filePath = System.IO.Path.Combine(appLocation, "FISAcops", "Students", "students.json");
        private static List<Student> students = new List<Student>();
        private int selectedStudent;


        private void SaveStudent_Click(object sender, RoutedEventArgs e)
        {
            // Ajouter un nouvel étudiant à la liste
            students[selectedStudent] = (new Student
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

            //retour à la page principale
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new Students());
        }

        //this function is for pre edit mail cause we need to be fast
        private void UpdateMail(object sender, RoutedEventArgs e)
        {
            string nom = nomTextBox.Text;
            string prenom = prenomTextBox.Text;

            // Construire la nouvelle adresse e-mail avec nom et prénom
            string newMail = $"{nom.ToLower()}.{prenom.ToLower()}@viacesi.fr";

            // Définir la nouvelle adresse e-mail dans le champ mailTextBox
            mailTextBox.Text = newMail;
        }


        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new Students());
        }

        public EditStudent(int selectedStudent)
        {
            this.selectedStudent = selectedStudent;
            InitializeComponent();

            // Charger les étudiants existants à partir du fichier JSON
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                students = JsonSerializer.Deserialize<List<Student>>(json);

                // Récupérer l'élève à partir de la liste des étudiants en utilisant l'indice
                var student = students[this.selectedStudent];

                // Remplir les champs du formulaire avec les valeurs de cet élève
                nomTextBox.Text = student.Nom;
                prenomTextBox.Text = student.Prenom;
                mailTextBox.Text = student.Mail;
                promoTextBox.Text = student.Promotion;
            }
        }

    }
}
