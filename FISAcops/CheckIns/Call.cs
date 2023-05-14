using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            /*
            List<Group> groups = GroupsService.LoadGroupsFromJson();
            Group? group = groups.FirstOrDefault(g => g.GroupName == groupName);

            if (group != null)
            {
                List<Student> students = group.StudentsList;
                foreach(Student student in students)
                {
                    StudentsWithState.Add((StudentWithState)StudentFactory.CreateStudent(student.Nom, student.Prenom, student.Mail, student.Promotion, "Controle"));
                }
            }
            */
        }

        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj != null && GetType() == obj.GetType())
            {
                Call otherCall = (Call)obj;
                result =  Date == otherCall.Date&& Time == otherCall.Time && GroupName == otherCall.GroupName && Frequency == otherCall.Frequency;
                if (!result) 
                {
                    if (otherCall.StudentsWithState.Count == StudentsWithState.Count) {
                        result = true;
                        for (int i = 0; i < otherCall.StudentsWithState.Count; i++)
                        {
                            if (StudentsWithState[i].Mail != otherCall.StudentsWithState[i].Mail ||
                                StudentsWithState[i].State != otherCall.StudentsWithState[i].State)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Time, GroupName, Frequency);
        }
    }
}
