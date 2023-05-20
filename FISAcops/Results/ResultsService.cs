using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FISAcops
{
    static class ResultsService
    {
        public static List<Result> LoadResultsFromJson(string date)
        {
            string resultsFilePath = Path.Combine(Settings.CallsPath, "Results" + date + ".json");

            // Vérifier si le fichier existe
            if (!File.Exists(resultsFilePath))
            {
                // Créer un fichier JSON vide
                CreateResultsJson(date);
            }

            var json = File.ReadAllText(resultsFilePath);
            var results = JsonSerializer.Deserialize<List<Result>>(json);
            results ??= new List<Result>();
            return results;
        }

        public static void SaveResultsToJson(List<Result> results, string date)
        {
            string resultsFilePath = Path.Combine(Settings.CallsPath, "Results" + date + ".json");

            // Vérifier si le fichier existe
            if (!File.Exists(resultsFilePath))
            {
                // Créer un fichier JSON vide
                CreateResultsJson(date);
            }

            var json = JsonSerializer.Serialize(results, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(resultsFilePath, json);
        }

        public static void CreateResultsJson(string date)
        {
            // Créer le dossier s'il n'existe pas
            Directory.CreateDirectory(Settings.CallsPath);

            string resultsFilePath = Path.Combine(Settings.CallsPath, "Results" + date + ".json");
            var json = "[]"; // JSON vide représentant une liste vide
            File.WriteAllText(resultsFilePath, json);
        }
    }
}
