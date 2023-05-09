using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Windows;

namespace FISAcops.CheckIns
{

    internal class CheckIn
    {
        public Student student;
        private readonly int code;

        public int GetCode() { return code; }

        public bool IsCodeGood(int enteredCode)
        {
            return code == enteredCode;
        }
        public string CodeMessage(int enteredCode)
        {
            string result;
            if (enteredCode == code)
            {
                result = $"{student.Prenom} {student.Nom} : Code bon";
            }
            else
            {
                result = $"{student.Prenom} {student.Nom} : Code incorrect";
            }
            return result;
        }

        public CheckIn(Student student, int inputCode) {
            this.student = student;
            this.code = inputCode;
        }
    }
}
