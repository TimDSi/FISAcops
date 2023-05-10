using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    internal class Call
    {
        public string GroupName { get; set; }
        public string DateHour { get; set; }

        public Call(string groupName, string dateHour)
        {
            GroupName = groupName;
            DateHour = dateHour;
        }
    }
}
