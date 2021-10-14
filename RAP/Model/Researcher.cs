using RAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Model;
using RAP.View;
namespace RAP
{
    public enum emp_level { Researcher, Student, A, B, C, D, E }
    class Researcher
    {
        //assign the var 
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Title_rdr { get; set; }
        public string Unit { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string Degree { get; set; }
        //public string Level { get; set; }
        public emp_level level { get; set; }
        public DateTime UTAS_start { get; set; }
        public DateTime Current_start { get; set; }
        public List<Researcher> supervisions_cal { get; set; }
        public string supervisorName { get; set; }
        public List<Position> pre_pos { get; set; }
        public List<Publication> Publications { get; set; }
        public double three_yr_avg { get; set; }
        public double Performance { get; set; }
        public string Supervisor { get; set; }

        //calcuate the period start with previous job
        public DateTime previous_job
        {
            get
            {
                if (pre_pos.Count > 0)
                {
                    var pres_date = from Position p in pre_pos
                                    orderby p.Start ascending
                                    select p.Start;


                    return pres_date.First();
                }
                return DateTime.Today;
            }
        }

        //calcuate the period start with current job
        public string current_job
        {
            get
            {
                return ToTitle(level);
            }

        }

        //set the title - choose
        public static string ToTitle(emp_level level)
        {
            switch (level)
            {
                case emp_level.A:
                    return "Postdoc";
                case emp_level.B:
                    return "Lecturer";
                case emp_level.C:
                    return "Senior Lecturer";
                case emp_level.D:
                    return "Associate Professor";
                case emp_level.E:
                    return "Professor";
                case emp_level.Student:
                    return "Student";
                default: return "Level has no associated title";
            }
        }

        //calcuate the tenure
        public double Tenure
        {
            get
            {
                return (double)((DateTime.Today - previous_job).Days) / 365;
            }
        }

        //calculate the performance with format
        public static double Performance_cal(emp_level level, double three_yr_avg)
        {
            switch (level)
            {
                case emp_level.A:
                    return three_yr_avg / 0.5 * 100;
                case emp_level.B:
                    return three_yr_avg / 1.0 * 100;
                case emp_level.C:
                    return three_yr_avg / 2.0 * 100;
                case emp_level.D:
                    return three_yr_avg / 3.2 * 100;
                case emp_level.E:
                    return three_yr_avg / 4.0 * 100;
                default: return 0;
            }
        }
        //supervision counts
        public int Supervisions
        {
            get { return supervisions_cal.Count; }
        }

        public int SkillCount
        {
            get { return Publications == null ? 0 : Publications.Count(); }
        }

        //The SkillCount out of 10, expressed as a percentage
        public double SkillPercent
        {
            //This is equivalent to SkillCount / 10.0 * 100
            get { return SkillCount * 10.0; }
        }

        //This is likely the solution you will have devised
        public DateTime MostRecentTraining
        {
            get
            {
                var skillDates = from Publication s in Publications
                                 orderby s.Certified descending
                                 select s.Certified;
                return skillDates.First();
            }
        }

        private List<CumulativeCount> cumulativeCounts = null;

        //calculate the cumulative count with publication list
        public List<CumulativeCount> CumulativePublicationCounts
        {
            get
            {
                if (cumulativeCounts == null)
                {
                    //One approach; not perfect since requires two passes
                    var counts = from p in Publications
                                 orderby p.Year
                                 group p by p.Year into byYear
                                 select new CumulativeCount { year = byYear.Key, count = byYear.Count() };
                    cumulativeCounts = counts.ToList();
                    int lastCount = 0;
                    foreach (CumulativeCount count in cumulativeCounts)
                    {
                        count.count += lastCount;
                        lastCount = count.count;
                    }
                }
                return cumulativeCounts;
            }
        }


        public override string ToString()
        {
            //For the purposes of this week's demonstration this returns only the name
            return Name;
        }
    }
}
