using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FISAcops
{
    static class ResultsService
    {
        public static List<Result> LoadResultsFromJson(string date)
        {
            string resultsFilePath = Path.Combine(new Settings().resultsPath, "Results" + date + ".json");
            var json = File.ReadAllText(resultsFilePath);
            var results = JsonSerializer.Deserialize<List<Result>>(json);
            results ??= new List<Result>();
            return results;
        }

        public static void SaveResultsToJson(List<Result> results, string date)
        {
            string resultsFilePath = Path.Combine(new Settings().resultsPath, "Results" + date + ".json");
            var json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(resultsFilePath, json);
        }
    }
}
