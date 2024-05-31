using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RedmineAutoLogTime.Models;

public class LogTimeList
{
    [JsonPropertyName("time_entries")]
    public List<LogTime> LogTimes { get; set; } = null!;
}