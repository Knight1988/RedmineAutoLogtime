using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedmineAutoLogTime.Interfaces.Services;
using RedmineAutoLogTime.Services;
using RedmineAutoLogTime.ViewModels;
using RedmineAutoLogTime.Workers;
using Serilog;

namespace RedmineAutoLogTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception");

            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices(ConfigureServices);
            builder.UseSerilog();
            _host = builder.Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("user-settings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3)
                .CreateLogger();
            
            services.AddSingleton(configuration);

            // ViewModels
            services.AddTransient<MainViewModel>();

            // Services
            services.AddSingleton<IRedmineService, RedmineService>();
            services.AddSingleton<IUserSettingService, UserSettingService>();
            services.AddSingleton<ISystemTrayService, SystemTrayService>();
            services.AddSingleton<IStartupService, StartupService>();

            // Windows
            services.AddSingleton<MainWindow>();

            // Workers
            services.AddHostedService<CheckLogWorker>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            ServiceLocator.Current = _host.Services;
            _host.Services.GetService<MainWindow>()!.Show();
            _host.Services.GetService<ISystemTrayService>()!.Init();
            _host.RunAsync();
        }
    }
}