using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RedmineAutoLogTime.Enums;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Services;

public class RedmineService : IRedmineService
{
    private readonly IConfiguration _configuration;

    public RedmineService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<Issue>> GetMyIssuesAsync()
    {
        var userApiKey = _configuration.GetValue<string>("UserApiKey");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", userApiKey);
        var response =
            await httpClient.GetStringAsync("https://redmine.vti.com.vn/issues.json?assigned_to_id=me&status_id=!*5|6");

        var issues = JsonSerializer.Deserialize<IssueList>(response);

        return issues!.Issues;
    }

    public async Task AddLogTimeToIssueAsync(string issueId, float hours, string comments, string spentOn,
        RedmineActivity redmineActivity)
    {
        var userApiKey = _configuration.GetValue<string>("UserApiKey")!;
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", userApiKey);

        var logTimeData = new
        {
            time_entry = new
            {
                issue_id = issueId,
                spent_on = spentOn,
                hours = hours,
                comments = comments,
                activity_id = (int)redmineActivity
            }
        };

        var json = JsonSerializer.Serialize(logTimeData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://redmine.vti.com.vn/time_entries.json", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to log time for issue {issueId}. Status code: {response.StatusCode}.");
        }
    }

    public async Task<List<LogTime>> GetTodayLogTimesAsync()
    {
        var userApiKey = _configuration.GetValue<string>("UserApiKey");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", userApiKey);
        var response =
            await httpClient.GetStringAsync("https://redmine.vti.com.vn/time_entries.json?spent_on=today&user_id=me");

        var logTimes = JsonSerializer.Deserialize<LogTimeList>(response);

        return logTimes!.LogTimes;
    }

    public async Task<Issue?> GetIssueByIdAsync(string issueId)
    {
        try
        {
            var userApiKey = _configuration.GetValue<string>("UserApiKey");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", userApiKey);
            var response = await httpClient.GetStringAsync($"https://redmine.vti.com.vn/issues/{issueId}.json");

            var issue = JsonSerializer.Deserialize<IssueList>(response);

            return issue?.Issue;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}