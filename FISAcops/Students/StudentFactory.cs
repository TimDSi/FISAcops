using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    public static class StudentFactory
    {
        public static IStudent CreateStudent(string nom, string prenom, string mail, string promotion, string? code)
        {
            if (code != null)
            {
                return new StudentWithCode(nom, prenom, mail, promotion, code);
            }
            else
            {
                return new Student(nom, prenom, mail, promotion);
            }
        }
    }
}
