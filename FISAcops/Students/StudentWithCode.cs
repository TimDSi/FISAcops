using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    internal class StudentWithCode : IStudent
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Promotion { get; set; }

        public StudentWithCode(string nom, string prenom, string mail, string promotion, string code)
        {
            Nom = nom;
            Prenom = prenom;
            Mail = mail;
            Promotion = promotion;
            Code = code;
        }

        public string Code { get; set; }

        public void UpdateCode(string newCode)
        {
            Code = newCode;
        }
    }
}
