using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Model
{
    class CumulativeCount
    {
        public int Year { get; set; }
        public int count { get; set; }

        //change the name of value, to make code readable and easy understood
        public CumulativeCount(int initialYear, int initialCount)
        {
            this.Year = initialYear;
            this.count = initialCount;
        }

        //After click the "Cumulative Count" button, display the information in the format as below.
        public override string ToString()
        {

            return "The cumulative count in " + Year + " is: " + count;


        }
    }
}
