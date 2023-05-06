namespace FISAcops
{
    public class Student
    {
        public Student(string nom, string prenom, string mail, string promotion)
        {
            Nom = nom;
            Prenom = prenom;
            Mail = mail;
            Promotion = promotion;
        }

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Promotion { get; set; }
    }
}