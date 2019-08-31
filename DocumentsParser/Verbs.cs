namespace DocumentsParser
{
    using Business.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Diagnostics;
    using System.IO;

    public static class Verbs
    {
        public static int RunCommand(ParallelForeachParserOptions parallelForeachParserOptions, ServiceProvider serviceProvider)
        {
            string[] files = Directory.GetFiles(parallelForeachParserOptions.LogsFolderPath);

            Stopwatch stopwatch = Stopwatch.StartNew();

            IParserService parserService = serviceProvider.GetService<ParallelForeachParserService>();

            parserService.RunParser(files);

            stopwatch.Stop();

            Console.Write($"Time execution on parse files {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine();

            Console.Write($"{ParallelForeachParserService.logModels.Count} logs line parsed from {files.Length} log files.");
            Console.ReadKey();

            return 0;
        }

        public static int RunCommand(BlockingCollectionParserOptions blockingCollectionParserOptions, ServiceProvider serviceProvider)
        {
            string[] files = Directory.GetFiles(blockingCollectionParserOptions.LogsFolderPath);

            Stopwatch stopwatch = Stopwatch.StartNew();

            IParserService parserService = serviceProvider.GetService<BlockingCollectionParserService>();

            parserService.RunParser(files);

            stopwatch.Stop();

            Console.Write($"Time execution on parse files {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine();
            
            Console.Write($"{ParallelForeachParserService.logModels.Count} logs line parsed from {files.Length} log files.");
            Console.ReadKey();

            return 0;
        }
    }
}
