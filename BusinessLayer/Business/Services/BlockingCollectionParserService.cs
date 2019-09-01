namespace Business.Services
{
    using Business.Models;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
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

        public void RunParser(string[] files, bool isSaveLogsToDB)
        {
            Task task1 = Task.Run(() =>
            {
                try
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    Parallel.ForEach(files, file =>
                    {
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

                    stopwatch.Stop();

                    Console.Write($"Time execution on parse files {stopwatch.ElapsedMilliseconds}");
                    Console.WriteLine();
                }
                finally
                {
                    lines.CompleteAdding();
                }
            });

            Task task2 = Task.Run(() =>
            {
                foreach (string line in lines.GetConsumingEnumerable())
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
                };

                Console.Write($"{logModels.Count} logs line parsed from {files.Length} log files.");
                Console.WriteLine();
            });

            Task.WaitAll(task1, task2);

            this.SaveLogsToDB(isSaveLogsToDB);
        }

        private void SaveLogsToDB(bool isSaveLogsToDB)
        {
            if (!isSaveLogsToDB)
            {
                return;
            }

            Console.Write($"Saving {logModels.Count} logs to database.");
            Console.WriteLine();

            Stopwatch stopwatch = Stopwatch.StartNew();

            logService.SaveLogs(logModels);

            stopwatch.Stop();

            Console.Write($"Time execution on save logs {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine();

            Console.Write($"End saving {logModels.Count} logs to database .");
            Console.WriteLine();
        }
    }
}
