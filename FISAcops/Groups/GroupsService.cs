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
            // Vérifier si le fichier existe
            if (!File.Exists(groupsFilePath))
            {
                // Créer un fichier JSON vide
                CreateGroupsJson();
            }

            var json = File.ReadAllText(groupsFilePath);
            var groups = JsonSerializer.Deserialize<List<Group>>(json);
            groups ??= new List<Group>();
            return groups;
        }

        public static void SaveGroupsToJson(List<Group> groups)
        {
            // Vérifier si le fichier existe
            if (!File.Exists(groupsFilePath))
            {
                // Créer un fichier JSON vide
                CreateGroupsJson();
            }

            var json = JsonSerializer.Serialize(groups, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(groupsFilePath, json);
        }

        //barrière de protection suplémentaire au cas ou le fichier n'existe pas.
        public static void CreateGroupsJson()
        {
            // Créer le dossier s'il n'existe pas
            Directory.CreateDirectory(Settings.GroupsPath);

            string groupsFilePath = Path.Combine(Settings.GroupsPath, "Groups.json");
            var json = "[]";
            File.WriteAllText(groupsFilePath, json);
        }
    }
}
