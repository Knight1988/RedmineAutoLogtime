using System;
using System.Diagnostics;

namespace RedmineAutoLogTime.Services;

public class StartupService : IStartupService
{
    public void SetRunOnStartup(bool enable)
    {
        var key =
            Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)!;
        if (enable)
        {
            var executePath = AppDomain.CurrentDomain.BaseDirectory;
            key.SetValue("RedmineAutoLogTime", executePath + "RedmineAutoLogTime.exe");
        }
        else
        {
            key.DeleteValue("RedmineAutoLogTime", false);
        }
    }
}