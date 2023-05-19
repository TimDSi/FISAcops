using System.Collections.Generic;

namespace FISAcops
{
    internal class StudentsPageMemento
    {
        public List<Student> Students { get; private set; }

        public StudentsPageMemento(List<Student> students)
        {
            Students = new List<Student>(students);
        }
    }
}
