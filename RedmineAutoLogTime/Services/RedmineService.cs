using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RedmineAutoLogTime.Enums;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Services;

public class RedmineService(IConfiguration configuration) : IRedmineService
{
    public async Task<bool> TestApiKeyAsync(string apiKey)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", apiKey);
        
        try
        {
            var response = await httpClient.GetStringAsync("https://redmine.vti.com.vn/users/current.json");
            return true;
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.Unauthorized || e.StatusCode == HttpStatusCode.Forbidden)
        {
            return false;
        }
    }

    public async Task AddLogTimeToIssueAsync(string issueId, float hours, string comments, string spentOn, Activity activity)
    {
        var userApiKey = configuration.GetValue<string>("apiKey")!;
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
                activity_id = activity.Id
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
        var userApiKey = configuration.GetValue<string>("apiKey");
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
            var userApiKey = configuration.GetValue<string>("apiKey");
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