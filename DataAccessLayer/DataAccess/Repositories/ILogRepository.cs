namespace DataAccess.Repositories
{
    using DataAccess.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILogRepository
    {
        Task SaveLog(Log log);

        void SaveLogs(List<Log> logs);
    }
}
