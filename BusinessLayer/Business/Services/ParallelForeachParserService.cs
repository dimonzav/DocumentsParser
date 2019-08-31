namespace Business.Services
{
    using Business.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParallelForeachParserService : IParserService
    {
        private readonly ILogService logService;

        public static List<LogModel> logModels = new List<LogModel>();

        public ParallelForeachParserService(ILogService logService)
        {
            this.logService = logService;
        }

        public void RunParser(string[] files)
        {
            Parallel.ForEach(files, f =>
            {
                Console.WriteLine();
                Console.Write($"Parse file {f} starts...");
                Console.WriteLine();

                this.ParseFile(f);

                Console.Write($"Parse file {f} ends.");
                Console.WriteLine();
            });
        }

        private void ParseFile(string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string line = reader.ReadLine();

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

                    logService.SaveLog(logModel).Wait();
                }
            }
        }

        public static IEnumerable<string> ReadAllLines(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
