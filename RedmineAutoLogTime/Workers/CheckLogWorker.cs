using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedmineAutoLogTime.Enums;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Workers;

public class CheckLogWorker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IRedmineService _redmineService;
    private readonly ILogger<CheckLogWorker> _logger;

    public CheckLogWorker(IConfiguration configuration, IRedmineService redmineService, ILogger<CheckLogWorker> logger)
    {
        _configuration = configuration;
        _redmineService = redmineService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckLogTimeAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred in CheckLogTimeAsync");
            }
        }
    }

    private async Task CheckLogTimeAsync(CancellationToken stoppingToken)
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
                try 
                {
                    await _redmineService.AddLogTimeToIssueAsync(issueId, 8, comment,
                        DateTime.Now.ToString("yyyy-MM-dd"), (RedmineActivity)activity);
                    MessageBox.Show("A logtime has been created!");
                } 
                catch(Exception e) 
                {
                    _logger.LogError(e, "An error occurred while adding logtime");
                }
            }
        }

        const int delayMinutes = 10;
        await Task.Delay(TimeSpan.FromMinutes(delayMinutes), stoppingToken);
    }
}