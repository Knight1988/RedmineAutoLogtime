using System.Diagnostics;

namespace RedmineAutoLogTime.Services;

public class StartupService : IStartupService
{
    public void SetRunOnStartup(bool enable)
    {
        var procStartInfo = new ProcessStartInfo("Startup.exe", enable.ToString())
        {
            UseShellExecute = true,
            Verb = "runas"
        };

        Process.Start(procStartInfo);
    }
}