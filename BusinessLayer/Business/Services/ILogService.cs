namespace Business.Services
{
    using Business.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILogService
    {
        Task SaveLog(LogModel logModel);

        void SaveLogs(List<LogModel> logModels);
    }
}
