using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FISAcops
{
    static class CallsService
    {
        public static string callsFilePath = Path.Combine(new Settings().callsPath, "Calls.json");
        public static List<Call> LoadCallsFromJson()
        {
            var json = File.ReadAllText(callsFilePath);
            var calls = JsonSerializer.Deserialize<List<Call>>(json);
            calls ??= new List<Call>();
            return calls;
        }

        public static void SaveGroupsToJson(List<Call> calls)
        {
            var json = JsonSerializer.Serialize(calls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(callsFilePath, json);
        }
    }
}
