using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SmartInvoicePrintingTool.Services.Abstractions;
using SmartInvoicePrintingTool.Utils;

namespace SmartInvoicePrintingTool.Services.Implementations;

public sealed class PdfMergingService : IPdfMergingService, IDisposable
{
    private readonly ILogger<PdfMergingService> _logger;
    private bool _disposed;

    public PdfMergingService(ILogger<PdfMergingService> logger) => _logger = logger;

    public async Task<bool> MergeAsync(
        string pdf1Path, double scale1,
        string pdf2Path, double scale2,
        string outputPath, CancellationToken ct = default)
    {
        PdfDocument outputDocument = null!;
        XPdfForm? form1 = null;
        XPdfForm? form2 = null;

        try
        {
            return await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                var meta1 = GetMetadata(pdf1Path);
                var meta2 = GetMetadata(pdf2Path);

                if (meta1 == null || meta2 == null) return false;

                outputDocument = new PdfDocument();
                var page = outputDocument.AddPage();
                page.Width = XUnit.FromPoint(PdfConstants.A4Width);
                page.Height = XUnit.FromPoint(PdfConstants.A4Height);

                var gfx = XGraphics.FromPdfPage(page);

                // 绘制第一个 PDF
                form1 = XPdfForm.FromFile(pdf1Path);
                var rect1 = new XRect(
                    0, 0,
                    meta1.Value.Width * scale1,
                    meta1.Value.Height * scale1);
                gfx.DrawImage(form1, rect1);

                // 绘制第二个 PDF
                form2 = XPdfForm.FromFile(pdf2Path);
                var rect2 = new XRect(
                    0, meta1.Value.Height * scale1 + PdfConstants.Spacing,
                    meta2.Value.Width * scale2,
                    meta2.Value.Height * scale2);
                gfx.DrawImage(form2, rect2);

                outputDocument.Save(outputPath);
                _logger.LogDebug("合并成功: {OutputPath}", outputPath);
                return true;
            }, ct);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("合并操作已取消");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "合并失败: {Pdf1} + {Pdf2}", pdf1Path, pdf2Path);
            return false;
        }
        finally
        {
            form2?.Dispose();
            form1?.Dispose();
            outputDocument?.Dispose();

            if (!_disposed)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }

    private (double Width, double Height)? GetMetadata(string path)
    {
        PdfDocument? doc = null;
        try
        {
            doc = PdfReader.Open(path, PdfDocumentOpenMode.Import);
            if (doc.PageCount == 0) return null;
            var page = doc.Pages[0];
            return (page.Width.Point, page.Height.Point);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "读取 PDF 失败: {Path}", path);
            return null;
        }
        finally
        {
            doc?.Close();
        }
    }

    public void Dispose() => _disposed = true;
}