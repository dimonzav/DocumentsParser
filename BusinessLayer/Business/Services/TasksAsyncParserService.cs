namespace Business.Services
{
    using Business.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TasksAsyncParserService : IParserService
    {
        private readonly ILogService logService;

        public static List<LogModel> logModels = new List<LogModel>();

        public TasksAsyncParserService(ILogService logService)
        {
            this.logService = logService;
        }

        public void RunParser(string[] files, bool isSaveLogsToDB)
        {
            Run(files, isSaveLogsToDB).Wait();
        }

        public async Task Run(string[] files, bool isSaveLogsToDB)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<Task> tasks = new List<Task>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(async () =>
                {
                    Console.Write($"Parse file {file} starts...");
                    Console.WriteLine();

                    List<LogModel> logModelsFromFile = await this.ParseFileAsync(file);

                    logModels.AddRange(logModelsFromFile);

                    Console.Write($"Parse file {file} ends.");
                    Console.WriteLine();
                }));
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();

            Console.Write($"Time execution on parse files {stopwatch.ElapsedMilliseconds}");
            Console.WriteLine();

            Console.Write($"{logModels.Count} logs line parsed from {files.Length} log files.");
            Console.WriteLine();
        }

        private async Task<List<LogModel>> ParseFileAsync(string file)
        {
            List<Task<LogModel>> tasks = new List<Task<LogModel>>();

            foreach (string line in File.ReadLines(file).Skip(1))
            {
                tasks.Add(Task.Run(() =>
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

                    return logModel;
                }));
            }

            LogModel[] logModels = await Task.WhenAll(tasks);

            return logModels.ToList();
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
