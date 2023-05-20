using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FISAcops
{
    static class GroupsService
    {
        public static string groupsFilePath = Path.Combine(Settings.CallsPath, "groups.json");
        public static List<Group> LoadGroupsFromJson()
        {
            var json = File.ReadAllText(groupsFilePath);
            var groups = JsonSerializer.Deserialize<List<Group>>(json);
            groups ??= new List<Group>();
            return groups;
        }

        public static void SaveGroupsToJson(List<Group> groups)
        {
            var json = JsonSerializer.Serialize(groups, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(groupsFilePath, json);
        }
    }
}
