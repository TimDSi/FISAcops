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

        private readonly string DefaultPath = Environment.CurrentDirectory;

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
            studentsPath = DefaultPath;
            groupsPath = DefaultPath;

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
            studentsPath = DefaultPath;
            groupsPath = DefaultPath;

            // Créer un objet JSON pour stocker les chemins des fichiers
            var jsonObject = new
            {
                studentsPath = DefaultPath,
                groupPath = DefaultPath
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


        private static void CreateSettingsFile(string defaultPath)
        {
            var settingsObject = new
            {
                StudentsPath = defaultPath,
                GroupPath = defaultPath
            };

            // Convertir l'objet en une chaîne JSON
            string jsonString = JsonSerializer.Serialize(settingsObject);

            // Écrire la chaîne JSON dans le fichier "settings.json"
            File.WriteAllText("settings.json", jsonString);
        }


        private void CreateStudentsFileIfNotExists()
        {
            if (!File.Exists(Path.Combine(studentsPath, "Students.json")))
            {
                var studentsObject = new[]
                {
                    new {
                        Nom = "Potter",
                        Prenom = "Harry",
                        Mail = "harry.potter@viacesi.fr",
                        Promotion = "Gryffondor"
                    },
                    new {
                        Nom = "Granger",
                        Prenom = "Hermione",
                        Mail = "hermione.granger@viacesi.fr",
                        Promotion = "Gryffondor"
                    },
                    new {
                        Nom = "Dragonneau",
                        Prenom = "Norbert",
                        Mail = "norbert.dragonneau@viacesi.fr",
                        Promotion = "Poufsouffle"
                    },
                    new {
                        Nom = "Malefoy",
                        Prenom = "Drago",
                        Mail = "drago.malefoy@viacesi.fr",
                        Promotion = "Serpentard"
                    },
                    new {
                        Nom = "Weasley",
                        Prenom = "Ron",
                        Mail = "ron.weasley@viacesi.fr",
                        Promotion = "Gryffondor"
                    },
                    new {
                        Nom = "Lovegood",
                        Prenom = "Luna",
                        Mail = "luna.lovegood@viacesi.fr",
                        Promotion = "Serdaigle"
                    },
                    new {
                        Nom = "Geignarde",
                        Prenom = "Mimi",
                        Mail = "mimi.geignarde@viacesi.fr",
                        Promotion = "Serdaigle"
                    },
                    new {
                        Nom = "Rogue",
                        Prenom = "Severus",
                        Mail = "severus.rogue@viacesi.fr",
                        Promotion = "Serpentard"
                    },
                    new {
                        Nom = "Weasley",
                        Prenom = "Ginny",
                        Mail = "ginny.weasley@viacesi.fr",
                        Promotion = "Gryffondor"
                    },
                    new {
                        Nom = "Black",
                        Prenom = "Sirius",
                        Mail = "sirius.black@viacesi.fr",
                        Promotion = "Gryffondor"
                    },
                    new {
                        Nom = "Chang",
                        Prenom = "Cho",
                        Mail = "cho.chang@viacesi.fr",
                        Promotion = "Serdaigle"
                    },
                    new {
                        Nom = "Tonks",
                        Prenom = "Nymphadora",
                        Mail = "nymphadora.tonks@viacesi.fr",
                        Promotion = "Poufsouffle"
                    },
                    new {
                        Nom = "Lestrange",
                        Prenom = "Bellatrix",
                        Mail = "bellatrix.lestrange@viacesi.fr",
                        Promotion = "Serpentard"
                    },
                    new {
                        Nom = "D'Silva",
                        Prenom = "Théotime",
                        Mail = "theotime.dsilva@viacesi.fr",
                        Promotion = "FISA_info_2225"
                    }
                };

                // Convertir l'objet en une chaîne JSON
                string jsonString = JsonSerializer.Serialize(studentsObject);

                // Écrire la chaîne JSON dans le fichier "students.json"
                File.WriteAllText(Path.Combine(studentsPath, "Students.json"), jsonString);
            }
        }

        private void CreateGroupsFileIfNotExists()
        {
            if (!File.Exists(Path.Combine(groupsPath, "Groups.json")))
            {
                var groupsObject = new[]
                {
                    new {
                        GroupName = "Gryffondor",
                        StudentsList = new []
                        {
                            new {
                                Nom = "Potter",
                                Prenom = "Harry",
                                Mail = "harry.potter@viacesi.fr",
                                Promotion = "Gryffondor"
                            },
                            new {
                                Nom = "Granger",
                                Prenom = "Hermione",
                                Mail = "hermione.granger@viacesi.fr",
                                Promotion = "Gryffondor"
                            },
                            new {
                                Nom = "Weasley",
                                Prenom = "Ron",
                                Mail = "ron.weasley@viacesi.fr",
                                Promotion = "Gryffondor"
                            },
                            new {
                                Nom = "Weasley",
                                Prenom = "Ginny",
                                Mail = "ginny.weasley@viacesi.fr",
                                Promotion = "Gryffondor"
                            },
                            new {
                                Nom = "Black",
                                Prenom = "Sirius",
                                Mail = "sirius.black@viacesi.fr",
                                Promotion = "Gryffondor"
                            }
                        }
                    },
                    new {
                        GroupName = "Serpentard",
                        StudentsList = new []
                        {
                            new {
                                Nom = "Malefoy",
                                Prenom = "Drago",
                                Mail = "drago.malefoy@viacesi.fr",
                                Promotion = "Serpentard"
                            },
                            new {
                                Nom = "Rogue",
                                Prenom = "Severus",
                                Mail = "severus.rogue@viacesi.fr",
                                Promotion = "Serpentard"
                            },
                            new {
                                Nom = "Lestrange",
                                Prenom = "Bellatrix",
                                Mail = "bellatrix.lestrange@viacesi.fr",
                                Promotion = "Serpentard"
                            }
                        }
                    },
                    new {
                        GroupName = "Poufsouffle",
                        StudentsList = new []
                        {
                            new {
                                Nom = "Dragonneau",
                                Prenom = "Norbert",
                                Mail = "norbert.dragonneau@viacesi.fr",
                                Promotion = "Poufsouffle"
                            },
                            new {
                                Nom = "Tonks",
                                Prenom = "Nymphadora",
                                Mail = "nymphadora.tonks@viacesi.fr",
                                Promotion = "Poufsouffle"
                            }
                        }
                    },
                    new {
                        GroupName = "Serdaigle",
                        StudentsList = new []
                        {
                            new {
                                Nom = "Lovegood",
                                Prenom = "Luna",
                                Mail = "luna.lovegood@viacesi.fr",
                                Promotion = "Serdaigle"
                            },
                            new {
                                Nom = "Geignarde",
                                Prenom = "Mimi",
                                Mail = "mimi.geignarde@viacesi.fr",
                                Promotion = "Serdaigle"
                            },
                            new {
                                Nom = "Chang",
                                Prenom = "Cho",
                                Mail = "cho.chang@viacesi.fr",
                                Promotion = "Serdaigle"
                            }
                        }
                    }
                };

                // Convertir l'objet en une chaîne JSON
                string jsonString = JsonSerializer.Serialize(groupsObject);

                // Écrire la chaîne JSON dans le fichier "students.json"
                File.WriteAllText(Path.Combine(groupsPath, "Groups.json"), jsonString);
            }
        }


        private void LoadSettings()
        {
            // Lire le contenu du fichier JSON
            string jsonString = File.ReadAllText(settingsPath);

            // Désérialiser le contenu JSON en un objet
            var jsonObject = JsonSerializer.Deserialize<dynamic>(jsonString);

            // Vérifier si la propriété GroupPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("GroupPath", out JsonElement groupPathElement))
            {
                // Récupérer la valeur de la propriété GroupPath
                string? filePath = groupPathElement.GetString();

                // Affecter la valeur de GroupPath à groupsPath
                if (filePath != null)
                {
                    groupsPath = filePath;
                }
            }
            CreateGroupsFileIfNotExists();

            // Vérifier si la propriété StudentsPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("StudentsPath", out JsonElement studentsPathElement))
            {
                // Récupérer la valeur de la propriété StudentsPath
                string? filePath = studentsPathElement.GetString();

                // Affecter la valeur de StudentsPath à studentsPath
                if (filePath != null)
                {
                    studentsPath = filePath;
                }
            }

            CreateStudentsFileIfNotExists();

            TxtGroupPath.Text = groupsPath;
            TxtStudentsPath.Text = studentsPath;
        }


        public Settings()
        {
            InitializeComponent();

            if (!File.Exists("settings.json"))
            {
                CreateSettingsFile(DefaultPath);
            }

            studentsPath = DefaultPath;
            groupsPath = DefaultPath;

            LoadSettings();
        }
    }
}
