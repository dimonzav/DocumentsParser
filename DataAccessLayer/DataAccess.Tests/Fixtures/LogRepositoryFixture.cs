namespace DataAccess.Tests.Fixtures
{
    using DataAccess.Entities;
    using DataAccess.Repositories;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class LogRepositoryFixture
    {
        private ILogRepository logRepository;

        private readonly Mock<DataContext> mockDataContext;

        public LogRepositoryFixture()
        {
            this.mockDataContext = new Mock<DataContext>();

            this.logRepository = new LogRepository(mockDataContext.Object);
        }

        [Fact]
        public void SaveLogTest_Success()
        {
            Log logEntity = new Log
            {
                Time = DateTime.Now,
                System = "System",
                User = "User",
                Event = "Event",
                Group = "Group",
                Viewers = "Viewers",
                Message = "Message"
            };

            int savedRecords = 1;

            this.mockDataContext.Setup(x => x.SaveChangesAsync(default(CancellationToken)))
                .ReturnsAsync(savedRecords)
                .Verifiable();

            logRepository.SaveLog(logEntity);

            mockDataContext.Verify(x => x.SaveChangesAsync(default(CancellationToken)), Times.Once());

            Assert.True(logEntity != null);
        }

        [Fact]
        public void SaveLogTest_Failed()
        {
            Log logEntity = new Log
            {
                Time = DateTime.Now,
                System = "System",
                User = "User",
                Event = "Event",
                Group = "Group",
                Viewers = "Viewers",
                Message = "Message"
            };

            int savedRecords = -1;

            this.mockDataContext.Setup(x => x.SaveChangesAsync(default(CancellationToken)))
                .ReturnsAsync(savedRecords)
                .Verifiable();

            Exception exception = Assert.Throws<Exception>(() => logRepository.SaveLog(logEntity));

            mockDataContext.Verify(x => x.SaveChangesAsync(default(CancellationToken)), Times.Once());
        }
    }
}
