namespace DocumentsParser
{
    using Business.Models;
    using Business.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public static class Verbs
    {
        public static int RunCommand(ParallelForeachParserOptions parallelForeachParserOptions, List<LogModel> logModels, ServiceProvider serviceProvider)
        {
            return RunCommand<ParallelForeachParserOptions, ParallelForeachParserService, LogModel>(parallelForeachParserOptions, logModels, serviceProvider);
        }

        public static int RunCommand(BlockingCollectionParserOptions blockingCollectionParserOptions, List<LogModel> logModels, ServiceProvider serviceProvider)
        {
            return RunCommand<BlockingCollectionParserOptions, BlockingCollectionParserService, LogModel>(blockingCollectionParserOptions, logModels, serviceProvider);
        }

        private static int RunCommand<TParserOptions, TParserService, TModel>(TParserOptions parserOptions, List<TModel> items, ServiceProvider serviceProvider)
            where TParserOptions : ParserOptions
            where TParserService : IParserService
            where TModel : class
        {
            string[] files = Directory.GetFiles(parserOptions.LogsFolderPath);

            IParserService parserService = serviceProvider.GetService<TParserService>();

            parserService.RunParser(files, parserOptions.IsSaveLogsToDB);

            return 0;
        }
    }
}
