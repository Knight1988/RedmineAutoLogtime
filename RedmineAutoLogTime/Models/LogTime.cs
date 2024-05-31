using System.Text.Json.Serialization;

namespace RedmineAutoLogTime.Models;

public class LogTime
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("hours")]
    public double Hours { get; set; }

    [JsonPropertyName("comments")]
    public string Comments { get; set; }

    [JsonPropertyName("spent_on")]
    public string SpentOn { get; set; }
}