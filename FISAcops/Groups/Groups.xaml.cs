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

        private string filePath = System.IO.Path.Combine(new Settings().groupsPath, "groups.json");

        private List<Group> groupsList = new();
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            /*
            // Récupère le groupe sélectionné
            Groupe groupeSelectionne = ((Button)sender).Tag as Groupe;

            // Ouvre la fenêtre pour modifier le groupe
            ModifierGroupe modifierGroupe = new ModifierGroupe(groupeSelectionne);
            modifierGroupe.Owner = Application.Current.MainWindow;
            modifierGroupe.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // Affiche la fenêtre en mode modal
            bool? dialogResult = modifierGroupe.ShowDialog();

            // Si l'utilisateur a cliqué sur le bouton "Modifier"
            if (dialogResult == true)
            {
                // Met à jour le groupe modifié
                groupeSelectionne.Nom = modifierGroupe.nomGroupeTextBox.Text;
                groupeSelectionne.Membres = modifierGroupe.membresGroupeTextBox.Text.Split(',').ToList();
                groupsListView.Items.Refresh();
            }
            */
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
            // Convertir la liste des étudiants en JSON
            string json = JsonSerializer.Serialize(groupList);

            // Enregistrer le JSON dans le fichier "^groups.json"
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
            groupsList = new List<Group>();
            groupsListView.ItemsSource = groupsList;
        }

    }
}
