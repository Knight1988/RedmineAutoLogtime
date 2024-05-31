using System.Text.Json.Serialization;

namespace RedmineAutoLogTime.Models;

public class Issue
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("subject")]
    public string? Subject { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}