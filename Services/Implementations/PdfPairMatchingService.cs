using System.Collections.Generic;
using System.Linq;
using SmartInvoicePrintingTool.Models;
using SmartInvoicePrintingTool.Services.Abstractions;

namespace SmartInvoicePrintingTool.Services.Implementations;

public class PdfPairMatchingService : IPdfPairMatchingService
{
    public List<PdfPair> MatchPairs(
        List<PdfMetadata> longPdfs,
        List<PdfMetadata> shortPdfs)
    {
        var pairs = new List<PdfPair>();
        var count = System.Math.Min(longPdfs.Count, shortPdfs.Count);

        // 按高排序
        var sortedLong = longPdfs.OrderBy(p => p.Height).ToList();
        var sortedShort = shortPdfs.OrderBy(p => p.Height).ToList();

        // 最长配最长，最短配最短
        for (int i = 0; i < count; i++)
        {
            pairs.Add(new PdfPair
            {
                LongPdf = sortedLong[i],
                ShortPdf = sortedShort[i]
            });
        }

        return pairs;
    }
}