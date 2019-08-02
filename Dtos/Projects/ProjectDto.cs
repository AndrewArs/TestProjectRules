using Newtonsoft.Json;

namespace Dtos.Projects
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Stage { get; set; }
        public int[] Categories { get; set; }
        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }
        [JsonProperty("modified_at")]
        public int ModifiedAt { get; set; }
    }
}
