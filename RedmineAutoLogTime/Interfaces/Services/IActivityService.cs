using System.Collections.Generic;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Interfaces.Services;

public interface IActivityService
{
    List<Activity>? ReadActivitiesFromJson();
}