using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartInvoicePrintingTool.Services.Abstractions;

namespace SmartInvoicePrintingTool.Services.Implementations;

/// <summary>
/// 使用 Windows 系统默认 PDF 打印服务。
/// 注意：此方式依赖于用户安装的 PDF 阅读器（如 Edge、Adobe Reader）。
/// </summary>
public sealed class PdfPrintingService : IPdfPrintingService
{
    private readonly ILogger<PdfPrintingService> _logger;

    public PdfPrintingService(ILogger<PdfPrintingService> logger) => _logger = logger;

    public Task<IReadOnlyList<string>> GetAvailablePrintersAsync()
    {
        // 注意：不使用 System.Drawing.Printing 依赖，改用 Win32 API 或 WMI 获取可能更好，
        // 但既然保留 System.Drawing，此处保留原引用
        try
        {
            return Task.FromResult<IReadOnlyList<string>>(
                System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取打印机列表失败");
            return Task.FromResult<IReadOnlyList<string>>(Array.Empty<string>());
        }
    }

    public Task<bool> PrintAsync(string pdfPath, string printerName, CancellationToken ct = default)
    {
        return Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                // 使用 Windows ShellExecute 机制调用关联的 PDF 程序打印
                // 这种方式最通用，只要系统装了 PDF 阅读器即可打印
                var psi = new ProcessStartInfo
                {
                    FileName = pdfPath,
                    Verb = "print", // 触发"打印"动作；若想指定打印机，部分程序支持 Arguments = printerName 且 Verb="printto"
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                using var process = Process.Start(psi);
                
                if (process == null)
                {
                    _logger.LogError("无法启动打印进程: {File}", pdfPath);
                    return false;
                }

                // 等待打印完成（或超时）
                // 注意：某些现代 PDF 阅读器（如 Edge）是异步处理打印的，启动即返回
                process.WaitForExit(5000);
                
                _logger.LogInformation("已发送打印任务: {File}", Path.GetFileName(pdfPath));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "打印任务发送失败: {File}", pdfPath);
                return false;
            }
        }, ct);
    }
}