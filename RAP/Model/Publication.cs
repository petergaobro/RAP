using RAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP
{
    public enum Mode { Conference, Journal, Other };
    public class Publication
    {
        public string DOI { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public Mode Mode { get; set; }
        public string Cite_as { get; set; }
        public DateTime Certified { get; set; }
        public string Authors { get; set; }

        public string Age => Math.Round(((double)((DateTime.Now - Certified).Days) / (double)365), 2).ToString();

        public int Freshness
        {
            //DateTime.Today returns today's date. As DateTime objects overload the
            //addition and subtraction operators we can use them to determine the
            //elapsed time between today's date and the Completed date. However, 
            //the result is not a number but a TimeSpan object, whose Days
            //property gives the number of whole days represented by the TimeSpan.
            get { return (DateTime.Today - Certified).Days; }
        }

        public override string ToString()
        {
            //This is a straightforward way of constructing the string using DateTime's
            //ToShortDateString method to remove the time component of the complted date
            return Title + " completed by " + Mode + " on " + Certified.ToShortDateString();
            //return Title;

            //This alternative approach uses the Format method of string, with the
            //short date format requested via the :d in the format string
            //return string.Format("{0} completed by {1} on {2:d}", Title, Mode, Certified);
        }
    }
}
