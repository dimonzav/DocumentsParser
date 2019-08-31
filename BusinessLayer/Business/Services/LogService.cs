namespace Business.Services
{
    using Business.Models;
    using DataAccess.Entities;
    using DataAccess.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LogService : ILogService
    {
        private readonly IParserRepository parserRepository;

        public LogService(IParserRepository parserRepository)
        {
            this.parserRepository = parserRepository;
        }

        public async Task SaveLog(LogModel logModel)
        {
            await this.parserRepository.SaveLog(logModel.ToEntity());
        }

        public void SaveLogs(List<LogModel> logModels)
        {
            List<Log> logs = ToList<Log, LogModel>(logModels);

            this.parserRepository.SaveLogs(logs);
        }

        private List<TEntity> ToList<TEntity, TModel>(List<TModel> models)
            where TEntity : class
            where TModel : IEntityModel<TEntity>, new()
        {
            List<TEntity> entities = new List<TEntity>();

            try
            {
                foreach (TModel model in models)
                {
                    entities.Add(model.ToEntity());
                }

                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while convert list of LogModel to Log list. Error: {ex.Message}.");
            }
        }
    }
}
