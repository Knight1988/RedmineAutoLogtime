using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RedmineAutoLogTime.Enums;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Workers;

public class CheckLogWorker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IRedmineService _redmineService;

    public CheckLogWorker(IConfiguration configuration, IRedmineService redmineService)
    {
        _configuration = configuration;
        _redmineService = redmineService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var logTimes = await _redmineService.GetTodayLogTimesAsync();
            var total = logTimes.Sum(l => l.Hours);
            // if total > 8 then message box 
            if (total > 8)
            {
                // Show message box 
                MessageBox.Show("You have logged more than 8 hours today!");
            }

            if (total == 0)
            {
                var activity = _configuration.GetValue<int>("activity");
                var comment = _configuration.GetValue<string>("comment");
                var issueId = _configuration.GetValue<string>("issueId");
                
                // add validate for configuration
                if (issueId != null && !string.IsNullOrEmpty(comment))
                {
                    await _redmineService.AddLogTimeToIssueAsync(issueId, 8, comment,
                        DateTime.Now.ToString("yyyy-MM-dd"), (RedmineActivity)activity);
                    MessageBox.Show("A logtime has been created!");
                }
            }

            const int delayMinutes = 10;
            await Task.Delay(TimeSpan.FromMinutes(delayMinutes), stoppingToken);
        }
    }
}