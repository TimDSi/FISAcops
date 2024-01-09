using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StudentEdition : Page
    {
        private static List<Student> studentsList = new();
        private readonly int selectedStudent;

        //Check if mail is already registered
        private static bool EmailExists(string email)
        {
            return studentsList.Any(s => s.Mail.ToLower() == email.ToLower());
        }

        private void SaveStudent_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si l'adresse e-mail existe déjà
            if (EmailExists(mailTextBox.Text))
            {
                MessageBox.Show("Cette adresse e-mail est déjà utilisée par un autre étudiant.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedStudent == -1) {
                // Ajouter un nouvel étudiant à la liste
                studentsList.Add((Student)StudentFactory.CreateStudent(
                    nomTextBox.Text,
                    prenomTextBox.Text,
                    mailTextBox.Text,
                    promoTextBox.Text,
                    null
                    ));
            }
            else
            {
                // Edit étudiant de la liste
                studentsList[selectedStudent] = (Student)StudentFactory.CreateStudent(
                    nomTextBox.Text,
                    prenomTextBox.Text,
                    mailTextBox.Text,
                    promoTextBox.Text,
                    null
                    );
            }
            StudentsService.SaveStudentsToJson(studentsList);
            ReturnAndLoad();
        }

        private static string RemoveDiacritics(string text)
        {
            return new string(
                text.Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray()
                );
        }

        //this function is for pre edit mail cause we need to be fast
        private void UpdateMail(object sender, RoutedEventArgs e)
        {
            //delete accent
            string? nom = RemoveDiacritics(nomTextBox.Text);
            string? prenom = RemoveDiacritics(prenomTextBox.Text);

            // Supprimer les caractères spéciaux
            nom = Regex.Replace(nom, "[^a-zA-Z]+", "");
            prenom = Regex.Replace(prenom, "[^a-zA-Z]+", "");

            // Construire la nouvelle adresse e-mail avec nom et prénom
            string newMail = $"{prenom.ToLower()}.{nom.ToLower()}@gmail.com";

            int number = 2;
            while (EmailExists(newMail))
            {
                newMail = $"{prenom.ToLower()}.{nom.ToLower()}{number}@gmail.com";
                number++;
            }

            // Définir la nouvelle adresse e-mail dans le champ mailTextBox
            mailTextBox.Text = newMail;
        }

        //return to previous page
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            ReturnAndLoad();
        }

        private void ReturnAndLoad()
        {
            // Récupérer la fenêtre courante
            var mainWindow = (MainWindow)Window.GetWindow(this);

            // Navigate to Students
            mainWindow.frame.Navigate(new Students());
        }

        public StudentEdition(int selectedStudent = -1)
        {
            InitializeComponent();
            this.selectedStudent = selectedStudent;
            // Charger les étudiants existants à partir du fichier JSON
            studentsList = StudentsService.LoadStudentsFromJson();

            // Vérifier si un élève est sélectionné ou non
            if (selectedStudent != -1 && studentsList != null)
            {
                // Récupérer l'élève à partir de la liste des étudiants en utilisant l'indice
                var student = studentsList[selectedStudent];

                // Remplir les champs du formulaire avec les valeurs de cet élève
                nomTextBox.Text = student.Nom;
                prenomTextBox.Text = student.Prenom;
                mailTextBox.Text = student.Mail;
                promoTextBox.Text = student.Promotion;
            }
        }
    }
}
