using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FISAcops
{
    static class StudentsService
    {
        public static string studentsPath = Path.Combine(new Settings().studentsPath, "students.json");

        public static List<Student> LoadStudentsFromJson()
        {
            var json = File.ReadAllText(studentsPath);
            var students = JsonSerializer.Deserialize<List<Student>>(json);
            students ??= new List<Student>();
            return students;
        }

        public static void SaveStudentsToJson(List<Student> students)
        {
            var studentsPath = Path.Combine(new Settings().studentsPath, "students.json");
            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(studentsPath, json);
        }
    }
}
