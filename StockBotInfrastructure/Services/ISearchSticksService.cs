namespace StockBotInfrastructure.Services;

public interface ISearchSticksService
{
    public Task Search(string timeInterval);
}