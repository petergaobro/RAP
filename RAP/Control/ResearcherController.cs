using RAP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RAP
{
    class ResearcherController
    {
        //The example shown here follows the pattern discussed in the Module 6 summary slides,
        //maintaining two collections, a master and a 'viewable' one (which is the 'view model'
        //in Microsoft's Model-View-ViewModel approach to Model-View-Controller)

        //visible stafflist
        private List<Researcher> staff;
        public List<Researcher> Workers { get { return staff; } set { } }

        private ObservableCollection<Researcher> viewableStaff;
        public ObservableCollection<Researcher> VisibleWorkers { get { return viewableStaff; } set { } }



        public ResearcherController()
        {
            staff = ERDAdapter.LoadAll();
            viewableStaff = new ObservableCollection<Researcher>(staff); //this list we will modify later

            //Part of step 2.3.2 from Week 8 tutorial
            foreach (Researcher e in staff)
            {
                e.Publications = ERDAdapter.LoadPublication(e.ID);
                e.pre_pos = ERDAdapter.LoadPostions(e.ID);
                e.three_yr_avg = Math.Round(ERDAdapter.fetchThreeYearCount(e.ID) / 3.0, 1);
                e.Performance = Researcher.Performance_cal(e.level, e.three_yr_avg);
                e.supervisions_cal = ERDAdapter.FetchSupervisions(e.ID);
                e.Supervisor = ERDAdapter.FetchSupervisorName(e.ID);
            }
        }

        public ObservableCollection<Researcher> GetViewableList()
        {
            return VisibleWorkers;
        }


        //Set filter by name
        public void FilterByName(String enteredName)
        {
            var selected = staff.Where(x => x.Name.ToLower().Contains(enteredName.ToLower())).ToList();
            viewableStaff.Clear();

            selected.ToList().ForEach(viewableStaff.Add);
        }
        // set the filter for employmentlevel
        public void FilterByLevel(emp_level selectedLevel)
        {

            if (selectedLevel == emp_level.Researcher)
            {
                viewableStaff.Clear();
                staff.ForEach(viewableStaff.Add);
            }
            else if (selectedLevel == emp_level.Student)
            {
                var selected = from Researcher r in staff
                               where r.Type == "Student"
                               select r;
                viewableStaff.Clear();

                selected.ToList().ForEach(viewableStaff.Add);
            }
            else
            {
                var selected = from Researcher r in staff
                               where r.level == selectedLevel
                               select r;
                viewableStaff.Clear();

                selected.ToList().ForEach(viewableStaff.Add);
            }
        }
    }
}
