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
            ServiceProvider serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddTransient<DataContext>()
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
                   (ParallelForeachParserOptions opts) => Verbs.RunCommand(opts, ParallelForeachParserService.logModels, serviceProvider),
                   (BlockingCollectionParserOptions opts) => Verbs.RunCommand(opts, BlockingCollectionParserService.logModels, serviceProvider),
                   (parserErrors) => 1);
        }
    }
}
