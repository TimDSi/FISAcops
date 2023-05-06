using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    internal class BaseStudent : Student
    {
        public BaseStudent(string nom, string prenom, string mail, string promotion)
            : base(nom, prenom, mail, promotion)
        {
            // Implémentez ici les propriétés et les méthodes spécifiques à l'élève de base
        }
    }
}
