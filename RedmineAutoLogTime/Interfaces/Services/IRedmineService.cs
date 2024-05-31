using System.Collections.Generic;
using System.Threading.Tasks;
using RedmineAutoLogTime.Enums;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Interfaces.Services;

public interface IRedmineService
{
    Task AddLogTimeToIssueAsync(string issueId, float hours, string comments, string spentOn,
        RedmineActivity redmineActivity);

    Task<List<LogTime>> GetTodayLogTimesAsync();
    Task<Issue?> GetIssueByIdAsync(string issueId);
    Task<bool> TestApiKeyAsync(string apiKey);
}