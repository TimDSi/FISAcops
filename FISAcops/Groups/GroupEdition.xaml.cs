using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GroupEdition : Page
    {
        private readonly List<Student> AvailableStudents = new();
        private readonly List<Student> SelectedStudents = new();

        private readonly List<Group> groupsList = new();
        private readonly int selectedGroup;

        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new Groups());
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
            if (checkBox.DataContext is Student student)
            {
            SelectedStudents.Add(student);
            }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
            if (checkBox.DataContext is Student student)
            {
            SelectedStudents.Remove(student);
            }
            }
        }


        private void SaveGroup_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";

            // Vérifier si le nom du groupe est déjà utilisé
            bool groupNameExists = groupsList.Any(group => group.GroupName == nomTextBox.Text);

            if (selectedGroup == -1)
            {
                if (groupNameExists)
                {
                    errorMessage = "Un groupe avec le même nom existe déjà. Veuillez choisir un nom différent.";
                }
                else
                {
                    // Ajouter un nouveau groupe à la liste
                    groupsList.Add(new Group(nomTextBox.Text, SelectedStudents));
                }
            }
            else
            {
                if (groupNameExists && groupsList[selectedGroup].GroupName != nomTextBox.Text)
                {
                    errorMessage = "Un groupe avec le même nom existe déjà. Veuillez choisir un nom différent.";
                }
                else
                {
                    // Modifier le groupe sélectionné dans la liste
                    groupsList[selectedGroup].GroupName = nomTextBox.Text;
                    groupsList[selectedGroup].StudentsList = SelectedStudents;
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GroupsService.SaveGroupsToJson(groupsList);

            ReturnAndLoad();
        }


        private void ReturnAndLoad()
        {
            // Récupérer la fenêtre courante
            var mainWindow = (MainWindow)Window.GetWindow(this);

            // Navigate to Students
            mainWindow.frame.Navigate(new Groups());
        }


        public GroupEdition(int selectedGroup = -1)
        {
            InitializeComponent();
            this.selectedGroup = selectedGroup;

            AvailableStudents = StudentsService.LoadStudentsFromJson();

            groupsList = GroupsService.LoadGroupsFromJson();
            if (selectedGroup == -1)
            {
                // Pas de groupe sélectionné, initialiser la liste des étudiants sélectionnés
                SelectedStudents = new List<Student>();
            }
            else if (groupsList != null)
            {
                // Groupe sélectionné, charger la liste des étudiants sélectionnés
                SelectedStudents = groupsList[selectedGroup].StudentsList;
                nomTextBox.Text = groupsList[selectedGroup].GroupName;
            }

            // Supprimer les étudiants sélectionnés de la liste AvailableStudents
            AvailableStudents?.RemoveAll(student => SelectedStudents.Any(s => s.Mail == student.Mail));

            dgStudents2.ItemsSource = AvailableStudents;
            dgStudents1.ItemsSource = SelectedStudents;

            if (selectedGroup == -1)
            {
                // Aucun groupe sélectionné, masquer la liste des étudiants sélectionnés
                dgStudents1.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Groupe sélectionné, afficher la liste des étudiants sélectionnés
                dgStudents1.Visibility = Visibility.Visible;
            }
        }
    }
}
