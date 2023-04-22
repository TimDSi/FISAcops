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
        private string filePath = @"D:\Workspace\VisualStudio\FISAcops\FISAcops\settings.json";
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
                TxtFolderPath.Text = selectedFolder;

                // Créer un objet JSON pour stocker le chemin de dossier
                var jsonObject = new
                {
                    settingsPath = selectedFolder
                };

                // Convertir l'objet JSON en une chaîne JSON
                string jsonString = JsonSerializer.Serialize(jsonObject);

                // Enregistrer la chaîne JSON dans un fichier
                File.WriteAllText(filePath, jsonString);
            }
        }

        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        private void BtnSetLocalFilePath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "settings.json";
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON Files (*.json)|*.json|All files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                // Utilisez le nouveau chemin d'accès pour le fichier "settings.json"
            }
        }

        public Settings()
        {
            InitializeComponent();

            if (File.Exists(filePath))
            {
                // Lire le contenu du fichier JSON
                string jsonString = File.ReadAllText(filePath);

                // Désérialiser le contenu JSON en un objet
                var jsonObject = JsonSerializer.Deserialize<dynamic>(jsonString);

                // Vérifier si la propriété settingsPath existe dans l'objet JSON
                if (jsonObject.TryGetProperty("settingsPath", out JsonElement settingsPathElement))
                {
                    // Récupérer la valeur de la propriété settingsPath
                    string settingsPath = settingsPathElement.GetString();

                    // Affecter la valeur de settingsPath à TxtFolderPath
                    TxtFolderPath.Text = settingsPath;
                }
            }
        }
    }
}
