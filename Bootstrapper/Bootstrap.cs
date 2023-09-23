using Bootstrapper.PostgresBootstrap;

namespace Bootstrapper;

public static class Bootstrap
{
    public static bool Run(PostgresOptions postgresOptions)
    {
        var postgresSuccessful = DbBootstrapper.Bootstrap(postgresOptions);
        return postgresSuccessful;
    }
}