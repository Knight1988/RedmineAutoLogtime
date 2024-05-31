using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RedmineAutoLogTime.Models;

public class IssueList
{
    [JsonPropertyName("issues")]
    public List<Issue> Issues { get; set; }
    
    [JsonPropertyName("issue")]
    public Issue Issue { get; set; }
}