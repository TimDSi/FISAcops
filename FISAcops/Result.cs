using System;
using System.Collections.Generic;

namespace FISAcops
{
    public class Result
    {
        public string Time { get; set; }
        public string GroupName { get; set; }
        public List<StudentWithState> StudentsWithStateList { get; set; }

        public Result(string time, string groupName, List<StudentWithState> studentsWithStateList)
        {
            Time = time;
            GroupName = groupName;
            StudentsWithStateList = studentsWithStateList;
        }
    }
}
