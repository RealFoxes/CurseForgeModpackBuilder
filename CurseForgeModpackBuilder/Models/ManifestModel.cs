using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CurseForgeModpackBuilder.Models
{

    public class ManifestModel
    {
        [JsonProperty("minecraft")]
        [JsonPropertyName("minecraft")]
        public Minecraft minecraft { get; set; }

        [JsonProperty("manifestType")]
        [JsonPropertyName("manifestType")]
        public string manifestType { get; set; }

        [JsonProperty("manifestVersion")]
        [JsonPropertyName("manifestVersion")]
        public int manifestVersion { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonProperty("version")]
        [JsonPropertyName("version")]
        public string version { get; set; }

        [JsonProperty("author")]
        [JsonPropertyName("author")]
        public string author { get; set; }

        [JsonProperty("files")]
        [JsonPropertyName("files")]
        public List<ManifestFile> files { get; set; }

        [JsonProperty("overrides")]
        [JsonPropertyName("overrides")]
        public string overrides { get; set; }
    }

    public class ManifestFile
    {
        [JsonProperty("projectID")]
        [JsonPropertyName("projectID")]
        public int projectID { get; set; }

        [JsonProperty("fileID")]
        [JsonPropertyName("fileID")]
        public int fileID { get; set; }

        [JsonProperty("required")]
        [JsonPropertyName("required")]
        public bool required { get; set; }
    }

    public class Minecraft
    {
        [JsonProperty("version")]
        [JsonPropertyName("version")]
        public string version { get; set; }

        [JsonProperty("modLoaders")]
        [JsonPropertyName("modLoaders")]
        public List<ModLoader> modLoaders { get; set; }
    }

    public class ModLoader
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonProperty("primary")]
        [JsonPropertyName("primary")]
        public bool primary { get; set; }
    }
}
