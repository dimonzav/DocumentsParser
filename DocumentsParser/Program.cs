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
            //args = new string[] { "c", "-f", @"C:\Users\dmytr\Desktop\log3" };

            ServiceProvider serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddTransient<DataContext>()
            .AddSingleton<ILogRepository, LogRepository>()
            .AddSingleton<BlockingCollectionParserService>()
            .AddSingleton<ParallelForeachParserService>()
            .AddSingleton<TasksAsyncParserService>()
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
                .ParseArguments<ParallelForeachParserOptions, BlockingCollectionParserOptions, TaskAsyncParserOptions>(args)
                   .MapResult(
                   (ParallelForeachParserOptions opts) => Verbs.RunCommand(opts, ParallelForeachParserService.logModels, serviceProvider),
                   (BlockingCollectionParserOptions opts) => Verbs.RunCommand(opts, BlockingCollectionParserService.logModels, serviceProvider),
                   (TaskAsyncParserOptions opts) => Verbs.RunCommand(opts, TasksAsyncParserService.logModels, serviceProvider),
                   (parserErrors) => 1);
        }
    }
}
