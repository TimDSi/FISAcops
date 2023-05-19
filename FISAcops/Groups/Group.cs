using System.Collections.Generic;

namespace FISAcops
{
    internal class Group
    {
        public string GroupName { get; set; }
        public List<Student> StudentsList { get; set; }

        public Group(string groupName, List<Student> studentsList)
        {
            GroupName = groupName;
            StudentsList = studentsList;
        }
    }
}
