using System;
using System.IO;

namespace RedmineAutoLogTime.Services;

public class StartupService : IStartupService
{
    public void SetRunOnStartup(bool enable)
    {
        // update registry to register this app for startup
        var key =
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)!;
        if (enable)
        {
            var executePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RedmineAutoLogTime.exe");
            key.SetValue("RedmineAutoLogTime", executePath);
        }
        else
        {
            key.DeleteValue("RedmineAutoLogTime", false);
        }
    }
}