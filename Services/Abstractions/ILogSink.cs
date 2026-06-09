using System;

namespace SmartInvoicePrintingTool.Services.Abstractions;

public interface ILogSink
{
    void Log(string message);
    event EventHandler<string>? LogMessage;
}