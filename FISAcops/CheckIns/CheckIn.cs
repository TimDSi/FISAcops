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
        private int code;

        public int getCode() { return code; }


        public string isCodeGood(int enteredCode)
        {
            string result;
            if (enteredCode == code)
            {
                result = "Code bon";
            }
            else
            {
                result = "Code incorrect";
            }
            return result;
        }

        public CheckIn() {
            code = new Random().Next(1, 100001);
        }
    }
}
