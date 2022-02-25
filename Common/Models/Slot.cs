using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Slot
    {
        public Slot(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }


        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
