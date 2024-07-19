using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;

using ImageReviewer;

namespace ImageReviewerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Add other configuration sources if needed
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddUserSecrets<App>();
                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    // Add other loggers as needed
                })
                .Build();

        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            // Register your services here
            services.AddSingleton<MainWindow>();
            services.AddScoped<YoloDataReader>();
            services.AddSingleton<YoloConfigManager>();
            // Add other services
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
            _host.Dispose();
            base.OnExit(e);
        }
    }

}
