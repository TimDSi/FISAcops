using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;


namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public string settingsPath = System.IO.Path.Combine(Environment.CurrentDirectory, "settings.json");
        public string studentsPath;
        public string groupsPath;


        private void SaveSettings(string studentsPath, string groupPath)
        {
            // Créer un objet JSON pour stocker les chemins de dossier
            var jsonObject = new
            {
                StudentsPath = studentsPath,
                GroupPath = groupPath
            };

            // Convertir l'objet JSON en une chaîne JSON
            string jsonString = JsonSerializer.Serialize(jsonObject);

            // Enregistrer la chaîne JSON dans un fichier
            File.WriteAllText(settingsPath, jsonString);
        }

        private void BtnSetFilePath_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings(studentsPath, groupsPath);
        }
        private void BtnStudentsPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Folder Selection";
            dialog.Filter = "Folders|*.none";
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (selectedFolder != null)
                {
                    studentsPath = selectedFolder;
                    TxtStudentsPath.Text = selectedFolder;
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            //Réinitialiser les valeurs par défaut
            studentsPath = @"C:\Users\33652\AppData\Local\FISAcops";
            groupsPath = @"C:\Users\33652\AppData\Local\FISAcops";

            // Mettre à jour les fichiers de configuration JSON
            SaveSettings(studentsPath, groupsPath);

            // Mettre à jour le texte du TextBox
            TxtGroupPath.Text = groupsPath;
            TxtStudentsPath.Text = studentsPath;
        }

        private void BtnGroupPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Folder Selection";
            dialog.Filter = "Folders|*.none";
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (selectedFolder != null)
                {
                    groupsPath = selectedFolder;
                    TxtGroupPath.Text = selectedFolder;
                }
            }
        }

        private void BtnSetLocalFilePath_Click(object sender, RoutedEventArgs e)
        {
            studentsPath = @"C:\Users\33652\AppData\Local\FISAcops";
            groupsPath = @"C:\Users\33652\AppData\Local\FISAcops";

            // Créer un objet JSON pour stocker les chemins des fichiers
            var jsonObject = new
            {
                studentsPath = studentsPath,
                groupPath = groupsPath
            };

            // Convertir l'objet JSON en une chaîne JSON
            string jsonString = JsonSerializer.Serialize(jsonObject);

            // Enregistrer la chaîne JSON dans un fichier
            File.WriteAllText(settingsPath, jsonString);
        }



        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        

        public Settings()
        {
            InitializeComponent();
            var settingsObject = new
            {
                StudentsPath = @"C:\Users\33652\AppData\Local\FISAcops",
                GroupPath = @"C:\Users\33652\AppData\Local\FISAcops"
            };

            // Convertir l'objet en une chaîne JSON
            string jsonString = JsonSerializer.Serialize(settingsObject);

            // Écrire la chaîne JSON dans le fichier "settings.json"
            File.WriteAllText("settings.json", jsonString);

            studentsPath = @"C:\Users\33652\AppData\Local\FISAcops";
            groupsPath = @"C:\Users\33652\AppData\Local\FISAcops";

            // Lire le contenu du fichier JSON
            jsonString = File.ReadAllText(settingsPath);

            // Désérialiser le contenu JSON en un objet
            var jsonObject = JsonSerializer.Deserialize<dynamic>(jsonString);

            
            // Vérifier si la propriété settingsPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("GroupPath", out JsonElement groupPathElement))
            {
                // Récupérer la valeur de la propriété settingsPath
                string? filePath = groupPathElement.GetString();

                // Affecter la valeur de settingsPath à TxtFolderPath
                if (filePath != null)
                {
                    groupsPath = filePath;
                }
            }

            // Vérifier si la propriété settingsPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("StudentsPath", out JsonElement studentsPathElement))
            {
                // Récupérer la valeur de la propriété settingsPath
                string? filePath = studentsPathElement.GetString();

                // Affecter la valeur de settingsPath à TxtFolderPath
                if (filePath != null)
                {
                    studentsPath = filePath;
                }
            }

            TxtGroupPath.Text = groupsPath;
            TxtStudentsPath.Text = studentsPath;
        }
    }
}
