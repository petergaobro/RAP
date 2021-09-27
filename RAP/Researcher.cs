using RAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP
{
    public enum emp_level { Researcher, Student, A, B, C, D, E }
    public class Researcher
    {
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
        public List<Publication> Skills { get; set; }
        public double three_yr_avg { get; set; }
        public double Performance { get; set; }
        public string Supervisor { get; set; }

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


        public string current_job
        {
            get
            {
                return ToTitle(level);
            }

        }

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
            //if (level == emp_level.A)
            //{ return "Postdoc"; }
            //else if (level == emp_level.B)
            //{ return "Lecturer"; }
            //else if (level == emp_level.C)
            //{ return "Senior Lecturer"; }
            //else if (level == emp_level.D)
            //{ return "Associate Professor"; }
            //else if (level == emp_level.E)
            //{ return "Professor"; }
            //return "Student";
        }

        public double Tenure
        {
            get
            {
                return (double)((DateTime.Today - previous_job).Days) / 365;
            }
        }

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

            //if (level == emp_level.A)
            //{ return three_yr_avg / 0.5 * 100; }
            //else if (level == emp_level.B)
            //{ return three_yr_avg / 1.0 * 100; }
            //else if (level == emp_level.C)
            //{ return three_yr_avg / 2.0 * 100; }
            //else if (level == emp_level.D)
            //{ return three_yr_avg / 3.2 * 100; }
            //else if (level == emp_level.E)
            //{ return three_yr_avg / 4.0 * 100; }
            //return 0;
        }
        //supervision counts
        public int Supervisions
        {
            get { return supervisions_cal.Count; }
        }

        public int SkillCount
        {
            get { return Skills == null ? 0 : Skills.Count(); }
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
                var skillDates = from Publication s in Skills
                                 orderby s.Certified descending
                                 select s.Certified;
                return skillDates.First();
            }
        }

        //This is a more robust implementation, but requires the the return type be made 'nullable'
        //        public DateTime? MostRecentTraining
        //        {
        //            get
        //            {
        //                if (SkillCount > 0)
        //                {
        //                    var skillDates = from TrainingSession s in Skills
        //                                     orderby s.Certified descending
        //                                     select s.Certified;
        //                    return skillDates.First();
        //                }
        //                return null;
        //            }
        //        }

        //public int supervisionCount
        //{
        //    get { return supervisions.Count; }
        //}

        public override string ToString()
        {
            //For the purposes of this week's demonstration this returns only the name
            //return ID;
            return Name;
        }
    }
}
