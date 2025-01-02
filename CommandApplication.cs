using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;
using RobinhoodCreateSignature.Commands;

namespace RobinhoodCreateSignature;

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
