using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Groups : Page
    {

        private readonly List<Group> groupsList = new();
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

                    // Supprimer visuellement le groupe du ListView
                    groupsListView.Items.Remove(selectedGroup);

                    // Enregistrer les modifications dans le fichier JSON
                    GroupsService.SaveGroupsToJson(groupsList);
                }
            }
        }
        
        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new GroupEdition());
        }


        public Groups()
        {
            InitializeComponent();
            groupsList = GroupsService.LoadGroupsFromJson();
            groupsListView.ItemsSource = groupsList;
        }

    }
}
