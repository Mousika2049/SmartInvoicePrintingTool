using SmartInvoicePrintingTool.Models;
using System.Collections.Generic;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface IPdfClassificationService
{
    (List<PdfMetadata> LongPdfs, List<PdfMetadata> ShortPdfs) ClassifyPdfs(
        IReadOnlyList<PdfMetadata> pdfs);
}