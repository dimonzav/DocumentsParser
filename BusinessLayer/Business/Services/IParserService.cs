namespace Business.Services
{
    public interface IParserService
    {
        void RunParser(string[] files, bool isSaveLogsToDB);
    }
}
