using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface IPdfPrintingService
{
    Task<IReadOnlyList<string>> GetAvailablePrintersAsync();
    Task<bool> PrintAsync(
        string pdfPath, string printerName, CancellationToken ct = default);
}