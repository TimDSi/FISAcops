using System;

namespace FISAcops
{
    public static class StudentFactory
    {
        public static IStudent CreateStudent(string nom, string prenom, string mail, string promotion, object obj)
        {
            if (obj is int code)
            {
                return new StudentWithCode(nom, prenom, mail, promotion, code);
            }
            else if (obj is string state)
            {
                return new StudentWithState(nom, prenom, mail, promotion, state);
            }
            else
            {
                return new Student(nom, prenom, mail, promotion);
            }
        }
    }
}
