using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SmartInvoicePrintingTool.App.Services;
using SmartInvoicePrintingTool.App.ViewModels;
using SmartInvoicePrintingTool.App.Views;

namespace SmartInvoicePrintingTool.App;

public partial class App : Application
{
    public static IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 配置 DI
        var services = new ServiceCollection();
        services.AddAppServices();
        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 获取 ViewModel
            var viewModel = Services.GetRequiredService<MainWindowViewModel>();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    public override void OnExit()
    {
        // 清理资源
        base.OnExit();
    }
}