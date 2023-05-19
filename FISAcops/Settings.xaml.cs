﻿using System;
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
        public string settingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");

        private static readonly string DefaultPath = Environment.CurrentDirectory;

        public string studentsPath = DefaultPath;
        public string groupsPath = DefaultPath;
        public string callsPath = DefaultPath;
        public string resultsPath = DefaultPath;

        public bool displayPopUpWhenCall = false;
        public string superviserEmail = "fisa.cops@gmail.com";
        public string superviserPassword = "MDPlongDonc5ecurise";

        private void SaveSettings(
            string studentsPath, 
            string groupsPath, 
            string callsPath, 
            string resultPath, 
            bool displayPopUpWhenCall, 
            string superviserEmail, 
            string superviserPassword
            )
        {
            // Créer un objet JSON pour stocker les chemins de dossier
            var jsonObject = new
            {
                StudentsPath = studentsPath,
                GroupsPath = groupsPath,
                CallsPath = callsPath,
                ResultsPath = resultPath,
                DisplayPopUpWhenCall = displayPopUpWhenCall,
                SuperviserEmail = superviserEmail,
                SuperviserPassword = superviserPassword
            };

            // Convertir l'objet JSON en une chaîne JSON
            string jsonString = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            // Enregistrer la chaîne JSON dans un fichier
            File.WriteAllText(settingsPath, jsonString);
        }




        //Btn change path --------------------------------------------------------------------------------------------------------
        private void BtnStudentsPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Folder Selection",
                Filter = "Folders|*.none",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = Path.GetDirectoryName(dialog.FileName);
                if (selectedFolder != null)
                {
                    studentsPath = selectedFolder;
                    TxtStudentsPath.Text = selectedFolder;
                }
            }
        }

        private void BtnGroupsPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Folder Selection",
                Filter = "Folders|*.none",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (selectedFolder != null)
                {
                    groupsPath = selectedFolder;
                    TxtGroupsPath.Text = selectedFolder;
                }
            }
        }

        private void BtnCallsPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Folder Selection",
                Filter = "Folders|*.none",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (selectedFolder != null)
                {
                    callsPath = selectedFolder;
                    TxtCallsPath.Text = selectedFolder;
                }
            }
        }

        private void BtnResultsPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "File Selection",
                Filter = "JSON Files|*.json",
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedFile = dialog.FileName;
                resultsPath = selectedFile;
                TxtResultsPath.Text = selectedFile;
            }
        }



        //-----------------------------------------------------------------------------------------------------------------

        // Email -------------------------------------------------------------------------------------------------

        private void PwdSupervisorPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            TxtSupervisorPassword.Text = PwdSupervisorPassword.Password;
        }

        private void TxtSupervisorPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            PwdSupervisorPassword.Password = TxtSupervisorPassword.Text;
        }

        private void BtnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (PwdSupervisorPassword.Visibility == Visibility.Visible)
            {
                PwdSupervisorPassword.Visibility = Visibility.Collapsed;
                TxtSupervisorPassword.Visibility = Visibility.Visible;
                TxtSupervisorPassword.Text = PwdSupervisorPassword.Password;
                BtnShowPassword.Content = " o o ";
            }
            else
            {
                PwdSupervisorPassword.Visibility = Visibility.Visible;
                TxtSupervisorPassword.Visibility = Visibility.Collapsed;
                BtnShowPassword.Content = " - - ";
            }
        }

        //--------------------------------------------------------------------------------------------------------------------


        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings(
                studentsPath, 
                groupsPath, 
                callsPath, 
                resultsPath, 
                ChkDisplayPopUpWhenCall.IsChecked == true,
                superviserEmail,
                superviserPassword
            );
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser les valeurs par défaut
            studentsPath = DefaultPath;
            groupsPath = DefaultPath;
            callsPath = DefaultPath;
            resultsPath= DefaultPath;
            displayPopUpWhenCall = false;
            superviserEmail = "fisa.cops@gmail.com";
            superviserPassword = "MDPlongDonc5ecurise";

            // Mettre à jour les fichiers de configuration JSON
            SaveSettings(
                studentsPath,
                groupsPath,
                callsPath,
                resultsPath,
                displayPopUpWhenCall,
                superviserEmail,
                superviserPassword
            );

            // Mettre à jour le texte du TextBox
            TxtGroupsPath.Text = groupsPath;
            TxtStudentsPath.Text = studentsPath;
            TxtCallsPath.Text = callsPath;
            TxtResultsPath.Text = resultsPath;
            ChkDisplayPopUpWhenCall.IsChecked = displayPopUpWhenCall;
            TxtSupervisorEmail.Text = superviserEmail;
            PwdSupervisorPassword.Password = superviserPassword;
        }



        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        //-----Create if not exists-------------------------------------------------------------------------------------------------------
        private static void CreateSettingsFileIfNotExists(string defaultPath)
        {
            if (!File.Exists("settings.json"))
            {
                var settingsObject = new
                {
                    StudentsPath = defaultPath,
                    GroupsPath = defaultPath,
                    CallsPath = defaultPath,
                    ResultsPath = defaultPath
                };

                // Convertir l'objet en une chaîne JSON
                string jsonString = JsonSerializer.Serialize(settingsObject);

                // Écrire la chaîne JSON dans le fichier "settings.json"
                File.WriteAllText("settings.json", jsonString);
            }
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

        private void CreateCallsFileIfNotExists()
        {
            if (!File.Exists(Path.Combine(callsPath, "Calls.json")))
            {
                var callObject = new[]
                {
                    new {
                        Date = "10/05/2023",
                        Time = "10:30",
                        GroupName = "Gryffondor",
                        Frequency = "Once"
                    },
                    new {
                        Date = "10/05/2023",
                        Time = "08:30",
                        GroupName = "Serpentar",
                        Frequency = "Weekly"
                    },
                    
                };

                // Convertir l'objet en une chaîne JSON
                string jsonString = JsonSerializer.Serialize(callObject);

                // Écrire la chaîne JSON dans le fichier "Call.json"
                File.WriteAllText(Path.Combine(callsPath, "Calls.json"), jsonString);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------

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


            // Vérifier si la propriété CallPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("CallsPath", out JsonElement callPathElement))
            {
                // Récupérer la valeur de la propriété CallPath
                string? filePath = callPathElement.GetString();

                // Affecter la valeur de CallPath à callPath
                if (filePath != null)
                {
                    callsPath = filePath;
                }
            }
            CreateCallsFileIfNotExists();


            // Vérifier si la propriété ResultsPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("ResultsPath", out JsonElement resultsPathElement))
            {
                // Récupérer la valeur de la propriété ResultsPath
                string? filePath = resultsPathElement.GetString();

                // Affecter la valeur de ResultsPath à resultsPath
                if (filePath != null)
                {
                    resultsPath = filePath;
                }
            }


            // Vérifier si la propriété DisplayPopUpWhenCall existe dans l'objet JSON
            if (jsonObject.TryGetProperty("DisplayPopUpWhenCall", out JsonElement displayPopUpWhenCallElement))
            {
                // Récupérer la valeur de la propriété DisplayPopUpWhenCall
                bool displayPopUpWhenCallValue = displayPopUpWhenCallElement.GetBoolean();

                // Affecter la valeur de DisplayPopUpWhenCall à displayPopUpWhenCall
                displayPopUpWhenCall = displayPopUpWhenCallValue;
            }

            // Vérifier si la propriété ResultsPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("SuperviserEmail", out JsonElement superviserEmailElement))
            {
                // Récupérer la valeur de la propriété ResultsPath
                string? superEmail = superviserEmailElement.GetString();

                // Affecter la valeur de ResultsPath à resultsPath
                if (superEmail != null)
                {
                    superviserEmail = superEmail;
                }
            }

            // Vérifier si la propriété ResultsPath existe dans l'objet JSON
            if (jsonObject.TryGetProperty("SuperviserPassword", out JsonElement supervierPasswordElement))
            {
                // Récupérer la valeur de la propriété ResultsPath
                string? superPassword = supervierPasswordElement.GetString();

                // Affecter la valeur de ResultsPath à resultsPath
                if (superPassword != null)
                {
                    superviserPassword = superPassword;
                }
            }


            TxtGroupsPath.Text = groupsPath;
            TxtStudentsPath.Text = studentsPath;
            TxtCallsPath.Text = callsPath;
            TxtResultsPath.Text = resultsPath;
            ChkDisplayPopUpWhenCall.IsChecked = displayPopUpWhenCall;
            TxtSupervisorEmail.Text = superviserEmail;
            PwdSupervisorPassword.Password = superviserPassword;
        }


        public Settings()
        {
            InitializeComponent();
            
            CreateSettingsFileIfNotExists(DefaultPath);

            LoadSettings();
        }
    }
}
