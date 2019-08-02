using System.Collections.Generic;
using Newtonsoft.Json;

namespace DomainModels.Models
{
    public class Effect
    {
        public string Type { get; set; }
        [JsonProperty("template_id")]
        public int TemplateId { get; set; }
        public Dictionary<string, string> Placeholders { get; set; }
    }
}
