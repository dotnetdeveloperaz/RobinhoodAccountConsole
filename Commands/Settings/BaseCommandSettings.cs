using Spectre.Console.Cli;
using System.ComponentModel;

namespace RobinhoodAccountConsole.Commands.Settings;

internal class BaseCommandSettings : CommandSettings
{
    [CommandOption("--debug")]
    [Description("Enable Debug Output")]
    [DefaultValue(false)]
    public bool Debug { get; set; } = false;

    [CommandOption("--hidden")]
    [Description("Enable User Secret Debug Output")]
    [DefaultValue(false)]
    public bool ShowHidden { get; set; } = false;

    [CommandOption("--apikey")]
    [Description("Provide Or Override The Api Key")]
    [DefaultValue(null)]
    public string? ApiKeyOverride { get; set; }
}
