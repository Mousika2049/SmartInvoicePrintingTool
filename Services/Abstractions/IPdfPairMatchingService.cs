using System.Collections.Generic;
using SmartInvoicePrintingTool.Models;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface IPdfPairMatchingService
{
    List<PdfPair> MatchPairs(
        List<PdfMetadata> longPdfs,
        List<PdfMetadata> shortPdfs);
}