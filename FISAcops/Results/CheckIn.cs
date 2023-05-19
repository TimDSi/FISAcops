namespace FISAcops.CheckIns
{
    internal class CheckIn
    {
        public StudentWithCode student;

        public int GetCode() { return student.Code; }

        public bool IsCodeGood(int enteredCode)
        {
            return student.Code == enteredCode;
        }
        public string CodeMessage(int enteredCode)
        {
            string result;
            if (enteredCode == student.Code)
            {
                result = $"{student.Prenom} {student.Nom} : Code bon";
            }
            else
            {
                result = $"{student.Prenom} {student.Nom} : Code incorrect";
            }
            return result;
        }

        public CheckIn(StudentWithCode student) {
            this.student = student;
        }
    }
}
