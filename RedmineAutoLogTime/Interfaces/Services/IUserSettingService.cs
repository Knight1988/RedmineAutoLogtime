using RedmineAutoLogTime.Models;

namespace RedmineAutoLogTime.Interfaces.Services;

public interface IUserSettingService
{
    UserSettings LoadUserSettings();
    void SaveUserSettings(UserSettings userSettings);
}