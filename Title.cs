
using System.Reflection;
using Spectre.Console;

namespace RobinhoodAccountConsole;

internal class Title
{
    public static void Print()
    {
        AssemblyName assembly = typeof(Title).Assembly.GetName();
        var version = $"v{assembly?.Version?.Major}.{assembly?.Version?.Minor}";
        Console.Clear();
        var titleTable = new Table().Centered();
        titleTable.AddColumn(
            new TableColumn(
                new Markup(
                    $"[yellow]Robinhood Account Console[/] {version}\r\n[green bold italic]Written By Scott Glasgow[/]"
                )
            ).Centered()
        );
        titleTable.BorderColor(Color.Yellow);
        titleTable.Border(TableBorder.Rounded);
        titleTable.Expand();

        AnsiConsole.Write(titleTable);
    }
}