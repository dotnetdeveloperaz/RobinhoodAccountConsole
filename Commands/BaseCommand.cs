using RobinhoodAccountConsole.Commands.Settings;
using RobinhoodAccountConsole.Models;
using Spectre.Console.Cli;

namespace RobinhoodAccountConsole.Commands;

internal class BaseCommand<T> : AsyncCommand<BaseCommandSettings>
{
    private readonly ApiServer _apiServer;
    internal BaseCommand(ApiServer apiServer)
    {
        _apiServer = apiServer;
    }
        protected async Task BaseExecuteAsync(CommandContext context, BaseCommandSettings settings)
    {
        // Run Common Tasks
        if (settings.Debug)
        {
            if (!DebugDisplay.Print(settings, _apiServer))
                return;
        }
        await Console.Out.WriteLineAsync("Execute Common Functionality");
    }

    public override async Task<int> ExecuteAsync(CommandContext context, BaseCommandSettings settings)
    {
        // Run common functionality
        await BaseExecuteAsync(context, settings);

        // Derived classes provide their specific implementation
        return await ExecuteDerivedAsync(context, settings);
    }
    protected virtual async Task<int> ExecuteDerivedAsync(CommandContext context, CommandSettings settings)
    {
        // Default Implementation to be overridden by derived classes
        await Console.Out.WriteLineAsync("BaseCommand");
        return 9;
    }
}
