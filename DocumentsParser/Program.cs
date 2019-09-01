namespace DocumentsParser
{
    using Business.Services;
    using CommandLine;
    using DataAccess;
    using DataAccess.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        static void Main(string[] args)
        {
            args = new[] { "a", "-f", @"C:\Users\dmytr\Desktop\log3" };
            
            ServiceProvider serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<DataContext>()
            .AddSingleton<ILogRepository, LogRepository>()
            .AddSingleton<BlockingCollectionParserService>()
            .AddSingleton<ParallelForeachParserService>()
            .AddSingleton<ILogService, LogService>()
            .BuildServiceProvider();

            using (DataContext context = new DataContext())
            {
                try
                {
                    context.Database.Migrate();
                }
                catch
                {
                    throw;
                }
            }

            Parser.Default
                .ParseArguments<ParallelForeachParserOptions, BlockingCollectionParserOptions>(args)
                   .MapResult(
                   (ParallelForeachParserOptions opts) => Verbs.RunCommand(opts, serviceProvider),
                   (BlockingCollectionParserOptions opts) => Verbs.RunCommand(opts, serviceProvider),
                   (parserErrors) => 1);
        }
    }
}
