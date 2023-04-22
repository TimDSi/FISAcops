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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public string settingsPath = @"C:\Users\33652\AppData\Local\FISAcops\settings.json";
        public string studentsPath = @"C:\Users\33652\AppData\Local\FISAcops\students.json";
        public string groupPath = @"C:\Users\33652\AppData\Local\FISAcops\group.json";

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
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
                string selectedFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (selectedFolder != null)
                {
                    updateFolder(selectedFolder);
                }

            }
        }

        private void updateFolder(string newFolder)
        {
            TxtFolderPath.Text = newFolder;

            // Créer un objet JSON pour stocker le chemin de dossier
            var jsonObject = new
            {
                filePath = newFolder
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

        private void BtnSetLocalFilePath_Click(object sender, RoutedEventArgs e)
        {
            var filePath = @"D:\Workspace\VisualStudio\FISAcops\FISAcops\settings.json";
            updateFolder(filePath);
        }

        public Settings()
        {
            InitializeComponent();

            if (File.Exists(settingsPath))
            {
                // Lire le contenu du fichier JSON
                string jsonString = File.ReadAllText(settingsPath);

                // Désérialiser le contenu JSON en un objet
                var jsonObject = JsonSerializer.Deserialize<dynamic>(jsonString);

                // Vérifier si la propriété settingsPath existe dans l'objet JSON
                if (jsonObject.TryGetProperty("filePath", out JsonElement settingsPathElement))
                {
                    // Récupérer la valeur de la propriété settingsPath
                    string filePath = settingsPathElement.GetString();

                    // Affecter la valeur de settingsPath à TxtFolderPath
                    TxtFolderPath.Text = filePath;
                }
            }
        }
    }
}
