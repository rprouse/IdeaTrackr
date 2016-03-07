using System;
using Humanizer;
using Newtonsoft.Json;

namespace IdeaTrackr.Model
{
    public class Idea
    {
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string DateDisplay => UpdatedAt.Humanize(true);
    }
}
