using System.Text.Json.Serialization;

namespace RedmineAutoLogTime.Models;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}