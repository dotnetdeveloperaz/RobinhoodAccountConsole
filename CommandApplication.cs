using Spectre.Console.Cli;
using RobinhoodAccountConsole.Commands;

namespace RobinhoodAccountConsole;

public class CommandApplication
{
    public static CommandApp Initialize(CommandApp app)
    {
        app.Configure(config =>
        {
            config.ValidateExamples();

            config
                .AddCommand<AccountCommand>("account")
                .WithDescription("Gets Account Information");
        });
        return app;
    }
}
