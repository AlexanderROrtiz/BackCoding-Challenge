using Amazon.CloudWatchLogs;
using BackCoding.Challenge.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.AwsCloudWatch;

namespace BackCoding.Challenge.WebApi.Extensions
{
    public static class AwsExtensions
    {
        public static void AddAwsMonitoring(this WebApplicationBuilder builder)
        {
            var awsClient = new AmazonCloudWatchLogsClient(Amazon.RegionEndpoint.USEast1);

            var options = new CloudWatchSinkOptions
            {
                LogGroupName = "/BackCoding/Logs",
                TextFormatter = new RenderedCompactJsonFormatter(),
                MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                CreateLogGroup = true
            };

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.AmazonCloudWatch(options, awsClient)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Health Check (RDS)
            builder.Services.AddHealthChecks()
                            .AddNpgSql(
                            builder.Configuration.GetConnectionString("DefaultConnection"),
                            name: "PostgreSQL",
                            tags: new[] { "db", "rds", "aws" });

        }
    }
}
