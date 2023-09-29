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