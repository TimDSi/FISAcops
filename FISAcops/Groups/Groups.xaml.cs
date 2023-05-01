using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Groups : Page
    {

        private readonly string filePath = System.IO.Path.Combine(new Settings().groupsPath, "groups.json");

        private List<Group> groupsList = new();
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'élève sélectionné
            var selectedGroupTag = ((Button)sender).Tag;
            int index = -1;

            // Rechercher l'indice de l'étudiant dans la liste
            if (selectedGroupTag != null)
            {
                Group selectedGroup = (Group)selectedGroupTag;
                index = groupsList.IndexOf(selectedGroup);
            }

            if (index >= 0)
            {
                // Naviguer vers la page d'édition de l'étudiant
                var mainWindow = (MainWindow)Window.GetWindow(this);
                mainWindow.frame.Navigate(new GroupEdition(index));
            }
            else
            {
                // L'étudiant sélectionné n'a pas été trouvé dans la liste
                // Gérer cette situation en conséquence
            }


        }


        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            // retrive groups list and selected group
            if (
                ((Button)sender).Tag is Group selectedGroup &&
                groupsListView.ItemsSource is List<Group> groupsList
                )
            {
                // Demander confirmation à l'utilisateur avant de supprimer l'élève
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer le groupe {selectedGroup.GroupName} ?", "Confirmation de suppression", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Supprimer l'élève de la liste des étudiants
                    groupsList.Remove(selectedGroup);

                    // Rafraîchir la liste des étudiants
                    groupsListView.ItemsSource = null;
                    groupsListView.ItemsSource = groupsList;

                    // Enregistrer les modifications dans le fichier JSON
                    SaveGroupsToJson(groupsList);
                }
            }
        }
        
        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new GroupEdition());
        }

        private void SaveGroupsToJson(List<Group> groupList)
        {
            // Convertir la liste des groupes en JSON
            string json = JsonSerializer.Serialize(groupList);

            // Écrire le JSON dans le fichier
            File.WriteAllText(filePath, json);
        }

        public void RefreshGroupsList()
        {
            // Charger les étudiants à partir du fichier JSON
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                groupsList = JsonSerializer.Deserialize<List<Group>>(json);
            }

            // Rafraîchir la source de données de la ListView
            groupsListView.ItemsSource = null;
            groupsListView.ItemsSource = groupsList;
        }

        public Groups()
        {
            InitializeComponent();
            RefreshGroupsList();
            groupsListView.ItemsSource = groupsList;
        }

    }
}
