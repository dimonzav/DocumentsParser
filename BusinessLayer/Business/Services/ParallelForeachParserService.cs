namespace Business.Services
{
    using Business.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public class ParallelForeachParserService : IParserService
    {
        private readonly ILogService logService;

        public static List<LogModel> logModels = new List<LogModel>();

        public ParallelForeachParserService(ILogService logService)
        {
            this.logService = logService;
        }

        public void RunParser(string[] files, bool isSaveLogsToDB)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.ForEach(files, f =>
            {
                Console.Write($"Parse file {f} starts...");
                Console.WriteLine();

                List<LogModel> logModelsFromFile = this.ParseFile(f);

                logModels.AddRange(logModelsFromFile);

                Console.Write($"Parse file {f} ends.");
                Console.WriteLine();
            });

            stopwatch.Stop();

            Console.Write($"Time execution on parse files {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine();

            Console.Write($"{logModels.Count} logs line parsed from {files.Length} log files.");
            Console.WriteLine();

            this.SaveLogsToDB(isSaveLogsToDB);
        }

        private List<LogModel> ParseFile(string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string line = reader.ReadLine();

                List<LogModel> logModels = new List<LogModel>();

                while ((line = reader.ReadLine()) != null)
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
                }

                return logModels;
            }
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
