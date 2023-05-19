namespace FISAcops
{
    internal class StudentWithCode : IStudent
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Promotion { get; set; }

        public StudentWithCode(string nom, string prenom, string mail, string promotion, int code)
        {
            Nom = nom;
            Prenom = prenom;
            Mail = mail;
            Promotion = promotion;
            Code = code;
        }

        public int Code { get; set; }

        public void UpdateCode(int newCode)
        {
            Code = newCode;
        }
    }
}
