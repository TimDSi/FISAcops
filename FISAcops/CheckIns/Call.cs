using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    public class Call
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string GroupName { get; set; }
        public string Frequency { get; set; }

        public Call(string date, string time, string groupName, string frequency)
        {
            Date = date;
            Time = time;
            GroupName = groupName;
            Frequency = frequency;
        }
    }
}
