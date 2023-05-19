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

        public bool IsResultDone()
        {
            bool result = true;
            foreach (var student in StudentsWithStateList)
            {
                if (student.State == "Controle")
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
