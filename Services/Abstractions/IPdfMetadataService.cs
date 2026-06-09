using System.Threading;
using System.Threading.Tasks;
using SmartInvoicePrintingTool.Models;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface IPdfMetadataService
{
    Task<PdfMetadata?> GetMetadataAsync(string pdfPath, CancellationToken ct = default);
}