using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
using RAP.Model;
using RAP.View;


namespace RAP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Part of task 3.4. If a resource really does need to be shared across many different views
        //then consider putting this code (and that for 3.4 below) into the App class, with a public
        //property to access the shared resource.
        private const string STAFF_LIST_KEY = "staffList";
        private ResearcherController research_controller;

        public MainWindow()
        {
            InitializeComponent();

            //Part of task 3.4 (yes, horribly long, but most of this won't change between different resources)
            // See App.xaml for an alternative way of declaring the Boss resource in two stages that would allow
            // this code to be simplified (as we could refer to the Boss object directly).
            research_controller = (ResearcherController)(Application.Current.FindResource(STAFF_LIST_KEY) as ObjectDataProvider).ObjectInstance;
        }

        // reasearcher selection
        private void researcherListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                //After Task 4 done, this is not really needed
                //MessageBox.Show("The selected item is: " + e.AddedItems[0]);
                //Part of task 4
                DetailsPanel.DataContext = e.AddedItems[0];
            }
        }
        //Drop down box for researcher list
        private void EmpLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (research_controller != null)
            {
                DetailsPanel.DataContext = null;
                PUB_DetailsPanel.DataContext = null;

                emp_level selectedLevel = (emp_level)e.AddedItems[0];

                research_controller.FilterByLevel(selectedLevel);
            }
        }

        // search bar by name -> filter
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DetailsPanel.DataContext = null;
            PUB_DetailsPanel.DataContext = null;

            var textbox = sender as TextBox;

            string enteredName = textbox.Text;

            if (enteredName != null)
            {
                research_controller.FilterByName(enteredName);
            }
        }

        // publication list selection
        private void publicationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                //After Task 4 done, this is not really needed
                //MessageBox.Show("The selected item is: " + e.AddedItems[0]);
                //Part of task 4
                PUB_DetailsPanel.DataContext = e.AddedItems[0];
            }
        }

        private void lnkSupervises_Click(object sender, RoutedEventArgs e)
        {
            lstSupervisions.Visibility = lstSupervisions.IsVisible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }
    }
}
