using System;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface IProgressReporter
{
    event EventHandler<double>? ProgressChanged;
    void Report(double percent);
}