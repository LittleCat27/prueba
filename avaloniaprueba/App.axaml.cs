using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using avaloniaprueba.Services;
using avaloniaprueba.ViewModels;
using avaloniaprueba.Views;

namespace avaloniaprueba;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var api = new ApiClient();
            var content = new ContentControl { Margin = new Thickness(20) };

            void ShowLogin()
            {
                content.Content = new LoginView
                {
                    DataContext = new LoginViewModel(api, ShowRegister, ShowLogs)
                };
            }

            void ShowRegister()
            {
                content.Content = new RegisterView
                {
                    DataContext = new RegisterViewModel(api, ShowLogin)
                };
            }

            async System.Threading.Tasks.Task ShowLogs()
            {
                var logs = new LogsViewModel(api, ShowLogin);
                content.Content = new LogsView { DataContext = logs };
                await logs.RefreshCommand.ExecuteAsync(null);
            }

            ShowLogin();
            desktop.MainWindow = new Window
            {
                Title = "avaloniaprueba", Width = 720, Height = 540,
                MinWidth = 600, MinHeight = 480,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = content
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}