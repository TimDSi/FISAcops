using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISAcops
{
    public interface IStudent
    {
        string Nom { get; set; }
        string Prenom { get; set; }
        string Mail { get; set; }
        string Promotion { get; set; }
    }
}
