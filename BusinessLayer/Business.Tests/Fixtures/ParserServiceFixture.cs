namespace Tests.Fixtures
{
    using Business.Services;
    using Moq;
    using System.IO;
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
        public void ParseFileTest()
        {
            string[] mockFiles = new string[]
            {
                "./TestFiles/logs_01.out",
                "./TestFiles/logs_02.out",
                "./TestFiles/logs_03.out"
            };

            int linesAmount = 0;

            foreach (string file in mockFiles)
            {
                string[] lines = File.ReadAllLines(file);

                // -1 from lines lenght is for remove first "Header" line
                linesAmount += (lines.Length - 1);
            }

            parserService.RunParser(mockFiles);

            Assert.True(ParallelForeachParserService.logModels.Count > 0);
            Assert.Equal(linesAmount, ParallelForeachParserService.logModels.Count);
        }
    }
}