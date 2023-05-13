using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    public static class StudentFactory
    {
        public static IStudent CreateStudent(string nom, string prenom, string mail, string promotion, string? obj)
        {
            if (int.TryParse(obj, out int code))
            {
                return new StudentWithCode(nom, prenom, mail, promotion, code.ToString());
            }
            else if (!string.IsNullOrEmpty(obj))
            {
                return new StudentWithState(nom, prenom, mail, promotion, obj);
            }
            else
            {
                return new Student(nom, prenom, mail, promotion);
            }
        }
    }
}
