namespace Tests.Fixtures
{
    using Business.Services;
    using Moq;
    using Xunit;

    public class ParserServiceFixture
    {
        private IParserService parserService;

        private Mock<ILogService> logService;

        public ParserServiceFixture()
        {
            this.logService = new Mock<ILogService>();

            this.parserService = new ParallelForeachParserService(logService.Object);
        }

        [Fact]
        public void ReadTraceFileTest()
        {
            string[] mockFiles = new string[]
            {
                "./TestFiles/logs_01.out",
                "./TestFiles/logs_02.out",
                "./TestFiles/logs_03.out"
            };
            
            parserService.RunParser(mockFiles);

            Assert.True(ParallelForeachParserService.logModels.Count > 0);
            Assert.Equal(300, ParallelForeachParserService.logModels.Count);
        }
    }
}