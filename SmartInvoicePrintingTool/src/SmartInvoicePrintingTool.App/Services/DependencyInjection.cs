using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartInvoicePrintingTool.App.ViewModels;
using SmartInvoicePrintingTool.App.Services.Abstractions;
using SmartInvoicePrintingTool.App.Services.Implementations;

namespace SmartInvoicePrintingTool.App.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // 1. 日志
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        // 2. 核心服务 (Singleton)
        services.AddSingleton<IPdfMetadataService, PdfMetadataService>();
        services.AddSingleton<IPdfPrintingService, PdfPrintingService>();
        // 后续步骤会添加更多服务

        // 3. ViewModels (Transient)
        services.AddTransient<MainWindowViewModel>();

        return services;
    }
}