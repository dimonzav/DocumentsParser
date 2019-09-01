namespace Business.Services
{
    using Business.Models;
    using System.Collections.Generic;

    public interface ILogService
    {
        void SaveLog(LogModel logModel);

        void SaveLogs(List<LogModel> logModels);
    }
}
