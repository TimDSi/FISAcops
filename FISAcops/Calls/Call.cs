using System;
using System.Collections.Generic;

namespace FISAcops
{
    public class Call
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string GroupName { get; set; }
        public string Frequency { get; set; }
        public List<StudentWithState> StudentsWithState { get; set; }


        public Call(string date, string time, string groupName, string frequency, List<StudentWithState> studentsWithState)
        {
            Date = date;
            Time = time;
            GroupName = groupName;
            Frequency = frequency;
            StudentsWithState = studentsWithState;
        }

        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj != null && GetType() == obj.GetType())
            {
                Call otherCall = (Call)obj;
                result =  Date == otherCall.Date&& Time == otherCall.Time && GroupName == otherCall.GroupName;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Time, GroupName, Frequency);
        }
    }
}
