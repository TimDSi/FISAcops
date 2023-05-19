using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FISAcops
{
    static class StudentsService
    {
        public static string studentsFilePath = Path.Combine(new Settings().studentsPath, "students.json");

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
    }
}
