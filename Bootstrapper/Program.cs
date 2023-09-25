using Bootstrapper;
using Bootstrapper.PostgresBootstrap;
using Microsoft.Extensions.Hosting;

using var host = Host
    .CreateDefaultBuilder(args)
    .Build();

var postgresOptions = new PostgresOptions
{
    ConnectionString = "Server=127.0.0.1;Port=5432;User id=stockbot;Password=password;Database=stockbot"
};

var isSuccessful =  Bootstrap.Run(postgresOptions);

Console.WriteLine($"Bootstrapper success: {isSuccessful.ToString().ToLower()}");


//Just testing if this works.. It does :) todo: Move to separate class

// var table_name = "sticks";
// var buy_levels = new[]{ 11232, 12343, 1234 };
// var sell_levels = new[]{ 5654, 7345, 7457 };
//
// var connection = new NpgsqlConnection(postgresOptions.ConnectionString);
//
// string commandText = $"INSERT INTO {table_name} (ticker, buy_levels, sell_levels) VALUES (@ticker, @buy_levels, @sell_levels)";
// await using (var cmd = new NpgsqlCommand(commandText, connection))
// {
//     cmd.Connection.Open();
//     cmd.Parameters.AddWithValue("ticker", "BTC");
//     cmd.Parameters.AddWithValue("buy_levels", buy_levels);
//     cmd.Parameters.AddWithValue("sell_levels", sell_levels);
//     await cmd.ExecuteNonQueryAsync();
//     cmd.Connection.Close();
//
// }