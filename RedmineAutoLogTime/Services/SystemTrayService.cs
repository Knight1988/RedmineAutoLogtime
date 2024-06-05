using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Hardcodet.Wpf.TaskbarNotification;

namespace RedmineAutoLogTime.Services;

public class SystemTrayService : ISystemTrayService
{
    private readonly MainWindow _mainWindow;
    private TaskbarIcon _taskbarIcon = null!;

    public SystemTrayService(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }
    
    public void Init()
    {
        _taskbarIcon = new TaskbarIcon();
        _taskbarIcon.Icon = new Icon("app.ico");
        _taskbarIcon.ToolTipText = "Redmine AutoLogTime";

        var contextMenu = new ContextMenu();

        var showItem = new MenuItem()
        {
            Header = "Show",
        };
        showItem.Click += (sender, args) => _mainWindow.Show();
        contextMenu.Items.Add(showItem);

        var exitItem = new MenuItem()
        {
            Header = "Exit",
        };
        exitItem.Click += (sender, args) => Exit();
        contextMenu.Items.Add(exitItem);

        _taskbarIcon.ContextMenu = contextMenu;
        _taskbarIcon.DoubleClickCommand = new RelayCommand(() => _mainWindow.Show());
    }

    private static void Exit()
    {
        Application.Current.Shutdown();
    }
}

public interface ISystemTrayService
{
    void Init();
}