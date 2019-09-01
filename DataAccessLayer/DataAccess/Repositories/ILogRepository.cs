namespace DataAccess.Repositories
{
    using DataAccess.Entities;
    using System.Collections.Generic;

    public interface ILogRepository
    {
        void SaveLog(Log log);

        void SaveLogs(List<Log> logs);
    }
}
