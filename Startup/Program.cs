// See https://aka.ms/new-console-template for more information

using Microsoft.Win32.TaskScheduler;

// Assume that you are passing argument 'true' or 'false' to enable SetRunOnStartup
string commandParam = null;
if (args.Any())
{
    commandParam = args[0];
}   
  
// Based on the command line argument run the SetRunOnStartup method  
if (!string.IsNullOrEmpty(commandParam) && bool.TryParse(commandParam, out bool shouldRunOnStartup))
{
    SetRunOnStartup(shouldRunOnStartup);
}
else
{
   Console.WriteLine("Invalid command parameter. Please, use either 'true' or 'false'.");
}


void SetRunOnStartup(bool enable)
{
    using var taskService = new TaskService();
    const string taskName = "RedmineAutoLogTime";
    if (enable)
    {
        var executePath = AppDomain.CurrentDomain.BaseDirectory;
        var td = taskService.NewTask();
        td.RegistrationInfo.Description = "Runs RedmineAutoLogTime at startup.";
        
        // Using logon trigger to start the app when the user logs on
        td.Triggers.Add(new LogonTrigger());
        
        // Setting the action to execute
        td.Actions.Add(new ExecAction(executePath + "RedmineAutoLogTime.exe"));
        
        // Register the task
        taskService.RootFolder.RegisterTaskDefinition(taskName, td);
    }
    else
    {
        // Remove the task
        taskService.RootFolder.DeleteTask(taskName);
    }
}