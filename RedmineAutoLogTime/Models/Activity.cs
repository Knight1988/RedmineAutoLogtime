using System.Text.Json.Serialization;

namespace RedmineAutoLogTime.Models;

public class Activity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}