using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartInvoicePrintingTool.ViewModels;
using SmartInvoicePrintingTool.Views;
using SmartInvoicePrintingTool.Services.Abstractions;
using SmartInvoicePrintingTool.Services.Implementations;

namespace SmartInvoicePrintingTool;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this); // 修正 1：添加 this
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = ConfigureServices(); // 修正 2：添加 var
        this.Services = services;
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services.AddLogging(builder =>
        {
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        // 注册服务
        services.AddSingleton<IPdfMetadataService, PdfMetadataService>();
        services.AddSingleton<IPdfClassificationService, PdfClassificationService>();
        services.AddSingleton<IPdfPairMatchingService, PdfPairMatchingService>();
        services.AddSingleton<IScaleCalculationService, ScaleCalculationService>();
        services.AddSingleton<IPdfMergingService, PdfMergingService>();
        services.AddSingleton<IPdfPrintingService, PdfPrintingService>();
        services.AddSingleton<ILogSink, ReactiveLogSink>();
        services.AddSingleton<IProgressReporter, ProgressReporter>();
        services.AddSingleton<IProcessingOrchestrator, ProcessingOrchestrator>();
        
        // 注册 ViewModel
        services.AddTransient<MainWindowViewModel>();
        
        return services.BuildServiceProvider();
    }
}