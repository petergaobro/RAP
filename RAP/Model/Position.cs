using RAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP
{
    public class Position
    {
        public emp_level Level { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public override string ToString()
        {
            if (End.HasValue)
            {
                //message display with the specific format
                return Start.ToShortDateString() + " - " + End.Value.ToShortDateString() + "   " + Researcher.ToTitle(Level);
            }
            else
            {
                return null;
            }
        }
    }
}
