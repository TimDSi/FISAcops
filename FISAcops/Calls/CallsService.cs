using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FISAcops
{
    static class CallsService
    {
        private static readonly object lockObject = new();
        public static string callsFilePath = Path.Combine(new Settings().callsPath, "Calls.json");

        public static List<Call> LoadCallsFromJson()
        {
            lock (lockObject)
            {
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
                var json = JsonSerializer.Serialize(calls, new JsonSerializerOptions
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
    }
}
