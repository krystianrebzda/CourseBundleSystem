using Newtonsoft.Json;

namespace CourseBundleSystem.Models;

public class TopicsRequest
{
    [JsonProperty("topics")]
    public Topics? Topics { get; set; }
}
