namespace Tests
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

            Mock<IParserService> mockFileManager = new Mock<IParserService>();
        }

        [Fact]
        public void ReadTraceFileTest()
        {
            Mock<IParserService> mockParserService = new Mock<IParserService>();

            string[] mockFiles = new string[]
            {
                "./TestFiles/logs_01.out",
                "./TestFiles/logs_02.out",
                "./TestFiles/logs_03.out"
            };

            //string[] passedFiles = null;

            //mockParserService.Setup(mockService => mockService.RunParser(mockFiles))
            //       .Callback((string[] files) =>
            //       {
            //           passedFiles = files;
            //       })
            //       .Verifiable();

            parserService.RunParser(mockFiles);

            Assert.True(ParallelForeachParserService.logModels.Count > 0);
            Assert.Equal(300, ParallelForeachParserService.logModels.Count);
        }
    }
}