namespace FISAcops
{
    public static class StudentFactory
    {
        public static IStudent CreateStudent(string nom, string prenom, string mail, string promotion, object? obj)
        {
            IStudent student;
            if (obj != null) {
                if (obj is int code)
                {
                    student = new StudentWithCode(nom, prenom, mail, promotion, code);
                }
                else if (obj is string state)
                {
                    student = new StudentWithState(nom, prenom, mail, promotion, state);
                }
                else
                {
                    student = new Student(nom, prenom, mail, promotion);
                }
            }
            else
            {
                student = new Student(nom, prenom, mail, promotion);
            }
            return student;
        }
    }
}
