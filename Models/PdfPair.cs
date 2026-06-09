namespace SmartInvoicePrintingTool.Models;

public record PdfPair
{
    public required PdfMetadata LongPdf { get; init; }      // 发票
    public required PdfMetadata ShortPdf { get; init; }     // 快递单
    public double LongScale { get; set; }    // 缩放比例
    public double ShortScale { get; set; }   // 缩放比例
    public string OutputFileName => $"{LongPdf.FileName}_{ShortPdf.FileName}.pdf";
}