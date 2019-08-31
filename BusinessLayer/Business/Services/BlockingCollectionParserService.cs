namespace Business.Services
{
    using Business.Models;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class BlockingCollectionParserService : IParserService
    {
        private readonly ILogService logService;

        private static BlockingCollection<string> lines = new BlockingCollection<string>();

        public static List<LogModel> logModels = new List<LogModel>();

        public BlockingCollectionParserService(ILogService logService)
        {
            this.logService = logService;
        }

        public void RunParser(string[] files)
        {
            Task task1 = Task.Run(() =>
            {
                try
                {
                    Parallel.ForEach(files, file =>
                    {
                        Console.WriteLine();
                        Console.Write($"Read file {file} starts...");
                        Console.WriteLine();

                        using (var reader = new StreamReader(file))
                        {
                            string line = reader.ReadLine();

                            while ((line = reader.ReadLine()) != null)
                            {
                                lines.Add(line);
                            }
                        }

                        Console.Write($"Read file {file} ends.");
                        Console.WriteLine();
                    });
                }
                finally
                {
                    lines.CompleteAdding();
                }
            });

            Task task2 = Task.Run(() =>
            {
                foreach(string line in lines.GetConsumingEnumerable())
                {
                    string[] lineSplit = line.Split("\t");

                    LogModel logModel = new LogModel();

                    if (lineSplit.Length == 7)
                    {
                        logModel.Time = DateTime.Parse(lineSplit[0]);
                        logModel.System = lineSplit[1];
                        logModel.User = lineSplit[2];
                        logModel.Event = lineSplit[3];
                        logModel.Group = lineSplit[4];
                        logModel.Viewers = lineSplit[5];
                        logModel.Message = lineSplit[6];
                    }

                    logModels.Add(logModel);

                    logService.SaveLog(logModel).Wait();
                };
            });

            Task.WaitAll(task1, task2);
        }
    }
}
