using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dtos.Projects
{
    public class ProjectDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Stage { get; set; }
        public List<long> Categories { get; set; }
        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
        [JsonProperty("modified_at")]
        public long ModifiedAt { get; set; }
    }
}
