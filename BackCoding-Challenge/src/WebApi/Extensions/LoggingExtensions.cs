using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.AwsCloudWatch;

namespace BackCoding.Challenge.WebApi.Extensions
{
    public static class LoggingExtensions
    {
        public static void AddCloudWatchLogging(this IHostBuilder host)
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

            host.UseSerilog();
        }
    }
}
