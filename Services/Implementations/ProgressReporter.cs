using System;
using SmartInvoicePrintingTool.Services.Abstractions;

namespace SmartInvoicePrintingTool.Services.Implementations;

public class ProgressReporter : IProgressReporter
{
    public event EventHandler<double>? ProgressChanged;
    public void Report(double percent) => ProgressChanged?.Invoke(this, percent);
}