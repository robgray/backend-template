﻿namespace Tests.Plumbing;

using Serilog;
using Serilog.Core;
using Xunit.Abstractions;

public static class TestSerilogLogger
{
    public static Logger CreateTestLogger(ITestOutputHelper output)
    {
        const string localSeqUrl = "http://localhost:5341";

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.Seq(localSeqUrl)
            .Enrich.FromLogContext();
        
        if (output is not null)
        {
            loggerConfiguration
                .WriteTo.TestOutput(output);
        }

        return loggerConfiguration.CreateLogger();
    }
}