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
        public async Task SaveLogTest_Success_Async()
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

            await logRepository.SaveLog(logEntity);

            mockDataContext.Verify(x => x.SaveChangesAsync(default(CancellationToken)), Times.Once());

            Assert.True(logEntity != null);
        }

        [Fact]
        public async Task SaveLogTest_Failed_Async()
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

            Exception exception = await Assert.ThrowsAsync<Exception>(async () => await logRepository.SaveLog(logEntity));

            mockDataContext.Verify(x => x.SaveChangesAsync(default(CancellationToken)), Times.Once());
        }
    }
}
