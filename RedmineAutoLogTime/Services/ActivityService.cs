using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Services;

public class ActivityService: IActivityService
{
    public List<Activity>? ReadActivitiesFromJson()
    {
        try
        {
            const string filePath = "activities.json";
            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Activity>>(jsonString);
        }
        catch (Exception e)
        {
            return null;
        }
    } 
}