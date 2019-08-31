namespace Business.Tests.Fixtures
{
    using Business.Models;
    using Business.Services;
    using DataAccess.Entities;
    using DataAccess.Repositories;
    using Moq;
    using System;
    using Xunit;

    public class LogServiceFixtures
    {
        private ILogService logService;

        private readonly Mock<ILogRepository> logRepository;

        public LogServiceFixtures()
        {
            this.logRepository = new Mock<ILogRepository>();

            this.logService = new LogService(logRepository.Object);
        }

        [Fact]
        public void SaveLogTest()
        {
            LogModel logModel = new LogModel
            {
                Time = DateTime.Now,
                System = "System",
                User = "User",
                Event = "Event",
                Group = "Group",
                Viewers = "Viewers",
                Message = "Message"
            };

            Log logEntity = null;

            logRepository.Setup(x => x.SaveLog(It.IsAny<Log>()))
                .Callback((Log log) =>
                {
                    logEntity = log;
                });

            logService.SaveLog(logModel);

            logRepository.Verify(x => x.SaveLog(It.IsAny<Log>()), Times.Once());

            Assert.True(logEntity != null);
            Assert.Equal(logModel.Time, logEntity.Time);
            Assert.Equal(logModel.System, logEntity.System);
            Assert.Equal(logModel.User, logEntity.User);
            Assert.Equal(logModel.Event, logEntity.Event);
            Assert.Equal(logModel.Group, logEntity.Group);
            Assert.Equal(logModel.Viewers, logEntity.Viewers);
            Assert.Equal(logModel.Message, logEntity.Message);
        }
    }
}
