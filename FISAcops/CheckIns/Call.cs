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

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Call otherCall = (Call)obj;

            return Date == otherCall.Date && Time == otherCall.Time && GroupName == otherCall.GroupName && Frequency == otherCall.Frequency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Time, GroupName, Frequency);
        }
    }
}
