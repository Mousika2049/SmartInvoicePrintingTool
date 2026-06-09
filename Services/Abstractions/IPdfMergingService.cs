using System.Threading;
using System.Threading.Tasks;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface IPdfMergingService
{
    Task<bool> MergeAsync(
        string pdf1Path, double scale1,
        string pdf2Path, double scale2,
        string outputPath, CancellationToken ct = default);
}