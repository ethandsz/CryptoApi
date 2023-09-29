using Npgsql;
using StockBotDomain.Models;

namespace StockBotInfrastructure.Repositories;

public class StickRepository : IStickRepository
{
    public async void AddLevels(List<HoldLevel> candleSticks)
    {
        var table_name = "sticks";
        var buy_levels = candleSticks.Where(h => !h.IsInverse);
        var sell_levels = candleSticks.Where(h => h.IsInverse);
        
        var connection = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User id=stockbot;Password=password;Database=stockbot");
        
        string commandText =
            $"INSERT INTO {table_name} (ticker, buy_levels, sell_levels) VALUES (@ticker, @buy_levels, @sell_levels)";
        await using (var cmd = new NpgsqlCommand(commandText, connection))
        {
            cmd.Connection?.Open();
            cmd.Parameters.AddWithValue("ticker", "BTC");
            cmd.Parameters.AddWithValue("buy_levels", buy_levels.Select(l => l.Level).ToList());
            cmd.Parameters.AddWithValue("sell_levels", sell_levels.Select(l => l.Level).ToList());
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection?.Close();
        }
    }

    public HoldLevel[] GetBuyLevels()
    {
        throw new NotImplementedException();
    }

    public HoldLevel[] GetSellLevels()
    {
        throw new NotImplementedException();
    }
}