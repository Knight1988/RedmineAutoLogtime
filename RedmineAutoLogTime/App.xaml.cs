using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WpfDependencyInjectionSample.ViewModels;

namespace RedmineAutoLogTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceLocator.Current = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            services.AddSingleton<IConfiguration>(builder.Build());

            // ViewModels
            services.AddTransient<MainViewModel>();

            // Windows
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceLocator.Current.GetService<MainWindow>();

            mainWindow.Show();
        }
    }
}