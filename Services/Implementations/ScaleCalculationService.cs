using System;
using SmartInvoicePrintingTool.Models;
using SmartInvoicePrintingTool.Services.Abstractions;
using SmartInvoicePrintingTool.Utils;

namespace SmartInvoicePrintingTool.Services.Implementations;

public class ScaleCalculationService : IScaleCalculationService
{
    public (double LongScale, double ShortScale)? CalculateScales(
        PdfMetadata longPdf, PdfMetadata shortPdf)
    {
        for (double longScale = PdfConstants.ScaleMax;
             longScale >= PdfConstants.ScaleMin;
             longScale -= PdfConstants.ScaleStep)
        {
            var longWidth = longPdf.Width * longScale;
            var longHeight = longPdf.Height * longScale;

            if (longWidth > PdfConstants.A4Width || longHeight > PdfConstants.A4Height)
                continue;

            var remainingHeight = PdfConstants.A4Height - longHeight - PdfConstants.Spacing;

            for (double shortScale = PdfConstants.ScaleMax;
                 shortScale >= PdfConstants.ScaleMin;
                 shortScale -= PdfConstants.ScaleStep)
            {
                var shortWidth = shortPdf.Width * shortScale;
                var shortHeight = shortPdf.Height * shortScale;

                if (shortWidth > PdfConstants.A4Width || shortHeight > remainingHeight)
                    continue;

                return (longScale, shortScale);
            }
        }

        return null;
    }
}