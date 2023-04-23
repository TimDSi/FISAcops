﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        private readonly string filePath = System.IO.Path.Combine(new Settings().studentsPath, "students.json");

        private static List<Student> studentsList = new();
        private readonly int? selectedStudent;

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

            if (selectedStudent == null) {
                // Ajouter un nouvel étudiant à la liste
                studentsList.Add(new Student
                {
                    Nom = nomTextBox.Text,
                    Prenom = prenomTextBox.Text,
                    Mail = mailTextBox.Text,
                    Promotion = promoTextBox.Text
                });
            }
            else
            {
                // Edit étudiant de la liste
                studentsList[(int)selectedStudent] = (new Student
                {
                    Nom = nomTextBox.Text,
                    Prenom = prenomTextBox.Text,
                    Mail = mailTextBox.Text,
                    Promotion = promoTextBox.Text
                });
            }
            

            // Sauvegarder la liste complète d'étudiants dans le fichier JSON
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true // Ajouter cette option pour formater le JSON de manière plus lisible
            };
            string output = JsonSerializer.Serialize(studentsList, options);
            File.WriteAllText(filePath, output, new UTF8Encoding(false));

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
            string nom = RemoveDiacritics(nomTextBox.Text);
            string prenom = RemoveDiacritics(prenomTextBox.Text);

            // Supprimer les caractères spéciaux
            nom = Regex.Replace(nom, "[^a-zA-Z]+", "");
            prenom = Regex.Replace(prenom, "[^a-zA-Z]+", "");

            // Construire la nouvelle adresse e-mail avec nom et prénom
            string newMail = $"{nom.ToLower()}.{prenom.ToLower()}@viacesi.fr";

            int number = 2;
            while (EmailExists(newMail))
            {
                newMail = $"{nom.ToLower()}.{prenom.ToLower()}{number}@viacesi.fr";
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
            mainWindow.frame.Navigate(new Groups());
        }

        public StudentEdition(int selectedStudent)
        {
            InitializeComponent();
            this.selectedStudent = selectedStudent;
            // Charger les étudiants existants à partir du fichier JSON
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                studentsList = JsonSerializer.Deserialize<List<Student>>(json);

                //supress warnings
                if (studentsList != null)
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

        public StudentEdition()
        {
            InitializeComponent();
            // Charger les étudiants existants à partir du fichier JSON
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                studentsList = JsonSerializer.Deserialize<List<Student>>(json);
            }
        }
    }
}
