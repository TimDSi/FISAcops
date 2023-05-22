using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FISAcops
{
    static class CallsService
    {
        private static readonly object lockObject = new();
        public static string callsFilePath = Path.Combine(Settings.CallsPath, "Calls.json");

        public static List<Call> LoadCallsFromJson()
        {
            lock (lockObject)
            {
                // Vérifier si le fichier existe
                if (!File.Exists(callsFilePath))
                {
                    // Créer un fichier JSON vide
                    CreateCallsJson();
                }

                var json = File.ReadAllText(callsFilePath);
                var calls = JsonSerializer.Deserialize<List<Call>>(json);
                calls ??= new List<Call>();
                return calls;
            }
        }

        public static void SaveCallsToJson(List<Call> calls)
        {
            lock (lockObject)
            {
                // Vérifier si le fichier existe
                if (!File.Exists(callsFilePath))
                {
                    // Créer un fichier JSON vide
                    CreateCallsJson();
                }

                List<Call> organizeCall = calls.OrderBy(call =>
                {
                    if (call.Frequency == "Once")
                    {
                        return 0;
                    }
                    else if (call.Frequency == "Daily")
                    {
                        return 1;
                    }
                    else if (call.Frequency == "Weekly")
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;
                    }
                }).ToList();


                var json = JsonSerializer.Serialize(organizeCall, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                File.WriteAllText(callsFilePath, json);
            }
        }

        public static List<Call> LoadCallsForSelectedDate(string selectedDate)
        {
            var calls = LoadCallsFromJson();
            var filteredCalls = new List<Call>();
            foreach (var call in calls)
            {
                if (call.Date == selectedDate)
                {
                    filteredCalls.Add(call);
                }
            }
            return filteredCalls;
        }

        //barrière de protection suplémentaire au cas ou le fichier n'existe pas.
        public static void CreateCallsJson()
        {
            // Créer le dossier s'il n'existe pas
            Directory.CreateDirectory(Settings.CallsPath);

            string callsFilePath = Path.Combine(Settings.CallsPath, "Calls.json");
            var json = "[]";
            File.WriteAllText(callsFilePath, json);
        }
    }
}
