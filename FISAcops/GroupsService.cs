using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FISAcops
{
    static class GroupsService
    {
        public static List<Group> LoadGroupsFromJson()
        {
            var groupPath = Path.Combine(new Settings().groupsPath, "groups.json");
            var json = File.ReadAllText(groupPath);
            var groups = JsonSerializer.Deserialize<List<Group>>(json);
            groups ??= new List<Group>();
            return groups;
        }

        public static void SaveGroupsToJson(List<Group> groups)
        {
            var groupPath = Path.Combine(new Settings().groupsPath, "groups.json");
            var json = JsonSerializer.Serialize(groups, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(groupPath, json);
        }
    }
}
