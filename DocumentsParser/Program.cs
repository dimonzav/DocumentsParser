namespace DocumentsParser
{
    using Business.Services;
    using CommandLine;
    using DataAccess.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        static void Main(string[] args)
        {
            args = new[] { "a", "-f", @"C:\Users\dmytr\Desktop\log3" };

            ServiceProvider serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<DataAccess.DataContext>()
            .AddSingleton<ILogRepository, LogRepository>()
            .AddSingleton<BlockingCollectionParserService>()
            .AddSingleton<ParallelForeachParserService>()
            .AddSingleton<ILogService, LogService>()
            .BuildServiceProvider();

            Parser.Default
                .ParseArguments<ParallelForeachParserOptions, BlockingCollectionParserOptions>(args)
                   .MapResult(
                   (ParallelForeachParserOptions opts) => Verbs.RunCommand(opts, serviceProvider),
                   (BlockingCollectionParserOptions opts) => Verbs.RunCommand(opts, serviceProvider),
                   (parserErrors) => 1);
        }
    }
}
