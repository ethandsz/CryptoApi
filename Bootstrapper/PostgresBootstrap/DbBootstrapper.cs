using System.Reflection;
using DbUp;

namespace Bootstrapper.PostgresBootstrap;

internal static class DbBootstrapper
{
    internal static bool Bootstrap(PostgresOptions options, bool logOutput = true)
    {
        EnsureDatabase.For.PostgresqlDatabase(options.ConnectionString);
        var engineBuilder =
            DeployChanges
                .To
                .PostgresqlDatabase(options.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    path => path.EndsWith(".sql", StringComparison.OrdinalIgnoreCase));

        if (logOutput)
        {
            engineBuilder.LogToConsole();
        }

        else
        {
            engineBuilder.LogToNowhere();
        }

        var result = engineBuilder
            .Build()
            .PerformUpgrade();
        
        return result.Successful;
    }
}

public record PostgresOptions
{
    public string? ConnectionString { get; init; }
}