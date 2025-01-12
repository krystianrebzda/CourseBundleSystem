using Newtonsoft.Json;

namespace CourseBundleSystem.Models;

public class Topics
{
    [JsonProperty("reading")]
    public int Reading { get; set; }

    [JsonProperty("math")]
    public int Math { get; set; }

    [JsonProperty("science")]
    public int Science { get; set; }

    [JsonProperty("history")]
    public int History { get; set; }

    [JsonProperty("art")]
    public int Art { get; set; }

    public Dictionary<string, int> ToDictionary()
    {
        return new Dictionary<string, int>
        {
            { "reading", Reading },
            { "math", Math },
            { "science", Science },
            { "history", History },
            { "art", Art }
        };
    }
}
