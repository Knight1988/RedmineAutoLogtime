using System;
using System.IO;
using System.Text.Json;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Services;

public class UserSettingService : IUserSettingService
{
    private const string UserSettingsFileName = "user-settings.json";

    public UserSettings LoadUserSettings()
    {
        try
        {
            if (!File.Exists(UserSettingsFileName)) return new UserSettings();

            var userSettingsJson = File.ReadAllText(UserSettingsFileName);
            return JsonSerializer.Deserialize<UserSettings>(userSettingsJson)!;
        }
        catch (Exception)
        {
            return new UserSettings();
        }
    }

    public void SaveUserSettings(UserSettings userSettings)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var jsonString = JsonSerializer.Serialize(userSettings, options);
        File.WriteAllText(UserSettingsFileName, jsonString);
    }
}