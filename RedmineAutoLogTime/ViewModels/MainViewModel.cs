using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RedmineAutoLogTime.Enums;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Messages;
using RedmineAutoLogTime.Models;
using RedmineAutoLogTime.Services;

namespace RedmineAutoLogTime.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly IRedmineService _redmineService;
    private readonly IUserSettingService _userSettingService;
    private readonly IStartupService _startupService;

    private RedmineActivity _selectedActivity;
    private string? _issueId;
    private string? _issueSubject;
    private bool _runOnStartup;
    private string? _apiKey;
    private string? _comment;

    public MainViewModel(IRedmineService redmineService, IUserSettingService userSettingService, IStartupService startupService)
    {
        _redmineService = redmineService;
        _userSettingService = userSettingService;
        _startupService = startupService;
        // load user settings
        var userSettings = userSettingService.LoadUserSettings();
        ApiKey = userSettings.ApiKey;
        Comment = userSettings.Comment;
        SelectedActivity = (RedmineActivity)userSettings.Activity;
        RunOnStartup = userSettings.RunOnStartup;
        IssueSubject = userSettings.IssueSubject;
        IssueId = userSettings.IssueId;
        
        WeakReferenceMessenger.Default.Register<TextChangedMessage>(this, OnTextChanged);
    }

    private async void OnTextChanged(object recipient, TextChangedMessage message)
    {
        if (message.Id == "IssueId" && !string.IsNullOrWhiteSpace(message.NewValue))
        {
            var issue = await _redmineService.GetIssueByIdAsync(message.NewValue);
            IssueSubject = issue?.Subject;
        }
    }

    public string? IssueId
    {
        get => _issueId;
        set => SetProperty(ref _issueId, value);
    }

    public string? IssueSubject
    {
        get => _issueSubject;
        set => SetProperty(ref _issueSubject, value);
    }

    public bool RunOnStartup
    {
        get => _runOnStartup;
        set => SetProperty(ref _runOnStartup, value);
    }

    public string? ApiKey
    {
        get => _apiKey;
        set => SetProperty(ref _apiKey, value);
    }

    public string? Comment
    {
        get => _comment;
        set => SetProperty(ref _comment, value);
    }

    public List<RedmineActivity> Activities { get; } = new()
    {
        RedmineActivity.Coding
    };

    public RedmineActivity SelectedActivity
    {
        get => _selectedActivity;
        set => SetProperty(ref _selectedActivity, value);
    }

    public ICommand ConnectCommand => new AsyncRelayCommand(ConnectAsync);

    private async Task ConnectAsync()
    {
        try
        {
            await _redmineService.GetMyIssuesAsync();
            MessageBox.Show("Connected successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception)
        {
            MessageBox.Show("Can't connect or API key incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public ICommand SaveCommand => new RelayCommand(Save);

    private void Save()
    {
        var userSettings = _userSettingService.LoadUserSettings();
        userSettings.ApiKey = ApiKey;
        userSettings.Comment = Comment;
        userSettings.Activity = (int)SelectedActivity;
        userSettings.RunOnStartup = RunOnStartup;
        userSettings.IssueSubject = IssueSubject;
        userSettings.IssueId = IssueId;
        _userSettingService.SaveUserSettings(userSettings);
        _startupService.SetRunOnStartup(RunOnStartup);
        MessageBox.Show("Save successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}