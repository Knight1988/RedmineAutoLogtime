using System.Text.Json.Serialization;

namespace RedmineAutoLogTime.Models;

public class UserSettings
{
    [JsonPropertyName("apiKey")]
    public string? ApiKey { get; set; }

    [JsonPropertyName("activity")]
    public int Activity { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("runOnStartup")]
    public bool RunOnStartup { get; set; }

    [JsonPropertyName("issueSubject")]
    public string? IssueSubject { get; set; }

    [JsonPropertyName("issueId")]
    public string? IssueId { get; set; }
}