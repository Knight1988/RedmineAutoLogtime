using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Workers;

public class CheckLogWorker(
    IConfiguration configuration,
    IRedmineService redmineService,
    ILogger<CheckLogWorker> logger)
    : BackgroundService
{
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
                logger.LogError(e, "An error occurred in CheckLogTimeAsync");
            }
        }
    }

    private async Task CheckLogTimeAsync(CancellationToken stoppingToken)
    {
        var logTimes = await redmineService.GetTodayLogTimesAsync();
        var total = logTimes.Sum(l => l.Hours);
        switch (total)
        {
            // if total > 8 then message box 
            case > 8:
                // Show message box 
                MessageBox.Show("You have logged more than 8 hours today!");
                break;
            case 0:
            {
                var activity = configuration.GetSection("activity").Get<Activity>();
                var comment = configuration.GetValue<string>("comment");
                var issueId = configuration.GetValue<string>("issueId");
                
                // add validate for configuration
                if (issueId != null && !string.IsNullOrEmpty(comment) && activity != null)
                {
                    try 
                    {
                        await redmineService.AddLogTimeToIssueAsync(issueId, 8, comment,
                            DateTime.Now.ToString("yyyy-MM-dd"), activity);
                        MessageBox.Show("A logtime has been created!");
                    } 
                    catch(Exception e) 
                    {
                        logger.LogError(e, "An error occurred while adding logtime");
                    }
                }

                break;
            }
        }

        const int delayMinutes = 10;
        await Task.Delay(TimeSpan.FromMinutes(delayMinutes), stoppingToken);
    }
}