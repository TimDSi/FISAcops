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
        private string Mail;
        private int code;

        public int getCode() { return code; }

        public bool isCodeGood(int enteredCode)
        {
            return code == enteredCode;
        }
        public string CodeMessage(int enteredCode)
        {
            string result;
            if (enteredCode == code)
            {
                result = Mail + " : Code bon";
            }
            else
            {
                result = Mail + " : Code incorrect";
            }
            return result;
        }

        public CheckIn(string Mail, int inputCode) {
            this.Mail = Mail;
            this.code = inputCode;
        }
    }
}
