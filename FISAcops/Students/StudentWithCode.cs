using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    internal class StudentWithCode : Student
    {
        public StudentWithCode(string nom, string prenom, string mail, string promotion, string code)
            : base(nom, prenom, mail, promotion)
        {
            Code = code;
        }

        public string Code { get; set; }

        public void UpdateCode(string newCode)
        {
            Code = newCode;
        }
    }
}
