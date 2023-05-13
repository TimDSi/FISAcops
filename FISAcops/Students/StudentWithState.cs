using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    public class StudentWithState : IStudent
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Promotion { get; set; }

        public string State { get; set; }

        public StudentWithState(string nom, string prenom, string mail, string promotion, string state)
        {
            Nom = nom;
            Prenom = prenom;
            Mail = mail;
            Promotion = promotion;
            State = state;
        }


        public void UpdateState(string newState)
        {
            State = newState;
        }
    }
}
