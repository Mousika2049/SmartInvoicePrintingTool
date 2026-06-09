using SmartInvoicePrintingTool.Models;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface IScaleCalculationService
{
    (double LongScale, double ShortScale)? CalculateScales(
        PdfMetadata longPdf, PdfMetadata shortPdf);
}