namespace DataAccess.Repositories
{
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LogRepository : ILogRepository
    {
        private readonly DataContext dataContext;

        public LogRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task SaveLog(Log log)
        {
            try
            {
                this.dataContext.Logs.Add(log);

                if (await this.dataContext.SaveChangesAsync() <= 0)
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
                this.dataContext.Logs.AddRange(logs);

                if (this.dataContext.SaveChanges() <= 0)
                {
                    throw new Exception("An error occurred while save logs to database.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while save logs to database. Error: {ex.Message}");
            }
        }
    }
}
