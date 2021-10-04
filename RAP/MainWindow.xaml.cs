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
        //private PublicationsController publication_controller = new PublicationsController();
        //private Publication pub;
        private Researcher res;

        public MainWindow()
        {
            InitializeComponent();

            //During step 2 it is necessary to hard code some values for the ListBox
            //sampleListBox.Items.Add("one");
            //sampleListBox.Items.Add("two");
            //sampleListBox.Items.Add("three");

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
            //if (e.AddedItems.Count > 0)
            //{
            //    pub = publication_controller.LoadPublcationDetail(((Publication)e.AddedItems[0]).DOI);
            //    PUB_DetailsPanel.DataContext = pub;
            //}
        }

        private void cumulative_Count_btn(object sender, RoutedEventArgs e)
        {
            if (res == null)
            {
                MessageBox.Show("Please select a researcher...");
            }
            else
            {
                if (res.Skills.Count == 0)
                {
                    MessageBox.Show("The researcher doesn't have any publication.");
                }
                else
                {
                    CumulativeCount cc2017 = new CumulativeCount(2017, 0);
                    CumulativeCount cc2016 = new CumulativeCount(2016, 0);
                    CumulativeCount cc2015 = new CumulativeCount(2015, 0);
                    CumulativeCount cc2014 = new CumulativeCount(2014, 0);
                    CumulativeCount cc2013 = new CumulativeCount(2013, 0);
                    CumulativeCount cc2012 = new CumulativeCount(2012, 0);
                    CumulativeCount cc2011 = new CumulativeCount(2011, 0);
                    CumulativeCount cc2010 = new CumulativeCount(2010, 0);

                    List<CumulativeCount> cc = new List<CumulativeCount>();

                    foreach (Publication pub in res.Skills)
                    {
                        if (pub.Year == 2017)
                        {
                            cc2017.count++;
                        }
                        else if (pub.Year == 2016)
                        {
                            cc2016.count++;
                        }
                        else if (pub.Year == 2015)
                        {
                            cc2015.count++;
                        }
                        else if (pub.Year == 2014)
                        {
                            cc2014.count++;
                        }
                        else if (pub.Year == 2013)
                        {
                            cc2013.count++;
                        }
                        else if (pub.Year == 2012)
                        {
                            cc2012.count++;
                        }
                        else if (pub.Year == 2011)
                        {
                            cc2011.count++;
                        }
                        else if (pub.Year == 2010)
                        {
                            cc2010.count++;
                        }
                    }

                    cc.Add(cc2017);
                    cc.Add(cc2016);
                    cc.Add(cc2015);
                    cc.Add(cc2014);
                    cc.Add(cc2013);
                    cc.Add(cc2012);
                    cc.Add(cc2011);
                    cc.Add(cc2010);

                    CumulativeCountView cumulative_count_view = new CumulativeCountView();
                    cumulative_count_view.cumulative_count_container.ItemsSource = cc;
                    var host = new Window();
                    //host.Width = 270;
                    //host.Height = 200;
                    host.Content = cumulative_count_view;
                    host.Show();
                }
            }
        }

        private void Supervisions_btn(object sender, RoutedEventArgs e)
        {
            if (res == null)
            {
                MessageBox.Show("Please select a researcher...");
            }
            else
            {
                if (res.supervisions_cal.Count == 0)
                {
                    MessageBox.Show("The researcher doesn't have any supervisions.");
                }
                else
                {
                    SupervisionListView supervision_list_view = new SupervisionListView();
                    supervision_list_view.supervision_container.ItemsSource = res.supervisions_cal;
                    var host = new Window();
                    host.Width = 270;
                    host.Height = 400;
                    host.Content = supervision_list_view;
                    host.Show();
                }
            }
        }



        //private void sampleButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("The text entered is: " + sampleTextBox.Text);
        //}

        //private void sampleTextBox_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        sampleButton_Click(sender, e);
        //    }
        //}

        //Part of task 3.4
        //private void btnDeleteOne_Click(object sender, RoutedEventArgs e)
        //{

        //    DetailsPanel.DataContext = new { Name = "Fred", SkillCount = 5 };


        //    if (boss.VisibleWorkers.Count > 0)
        //    {
        //        Employee theRemoved = boss.VisibleWorkers[0]; //this is just to keep the GUI tidy (after Task 4 implemented)
        //        boss.VisibleWorkers.RemoveAt(0); //the actual removal step
        //        //completing keeping the GUI tidy (something similar may be required in the assignment)
        //        if (DetailsPanel.DataContext == theRemoved)
        //        {
        //            DetailsPanel.DataContext = null;
        //        }
        //    }
        //}

        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}
    }
}
