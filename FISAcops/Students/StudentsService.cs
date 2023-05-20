using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FISAcops
{
    static class StudentsService
    {
        public static string studentsFilePath = Path.Combine(Settings.StudentsPath, "students.json");

        public static List<Student> LoadStudentsFromJson()
        {
            var json = File.ReadAllText(studentsFilePath);
            var students = JsonSerializer.Deserialize<List<Student>>(json);
            students ??= new List<Student>();
            return students;
        }

        public static void SaveStudentsToJson(List<Student> students)
        {
            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(studentsFilePath, json);
        }

        //barrière de protection suplémentaire au cas ou le fichier n'existe pas.
        public static void CreateStudentsJson()
        {
            // Créer le dossier s'il n'existe pas
            Directory.CreateDirectory(Settings.StudentsPath);

            string studentsFilePath = Path.Combine(Settings.StudentsPath, "Students.json");
            var json = "[]";
            File.WriteAllText(studentsFilePath, json);
        }
    }
}
