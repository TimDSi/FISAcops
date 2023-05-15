using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
