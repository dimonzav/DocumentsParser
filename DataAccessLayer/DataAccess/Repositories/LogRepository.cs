namespace DataAccess.Repositories
{
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LogRepository : ILogRepository
    {
        private readonly DataContext dataContext;

        public LogRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void SaveLog(Log log)
        {
            try
            {
                this.dataContext.Logs.Add(log);

                if (this.dataContext.SaveChanges() <= 0)
                {
                    throw new Exception("An error occurred while save log to database.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while save log to database. Error: {ex.Message}");
            }
        }

        public void SaveLogs(List<Log> logs)
        {
            try
            {
                try
                {
                    int count = logs.Count;

                    List<List<Log>> bulkList = new List<List<Log>>();

                    int skip = 0;

                    while (count > 0)
                    {
                        if (count < 1000)
                        {
                            bulkList.Add(logs.Skip(skip).Take(count).ToList());
                        }
                        else
                        {
                            bulkList.Add(logs.Skip(skip).Take(1000).ToList());
                        }
                        
                        count -= 1000;

                        skip += 1000;
                    }

                    Parallel.ForEach(bulkList, bulk =>
                    {
                        AddToContext(bulk);
                    });
                }
                catch
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while save logs to database. Error: {ex.Message}");
            }
        }

        private void AddToContext(List<Log> entities)
        {
            using (DataContext dataContext = new DataContext())
            {
                //dataContext.AddRange(entities);

                //dataContext.SaveChanges();

                dataContext.BulkInsert(entities, op => op.AutoMapOutputDirection = false);
            }
        }
    }
}
