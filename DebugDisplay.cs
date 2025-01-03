using RobinhoodAccountConsole.Models;
using Spectre.Console.Cli;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RobinhoodAccountConsole
{
    internal class DebugDisplay
    {
        internal static bool Print(CommandSettings settings, ApiServer server)
        {
            // Debug Window
            var table = new Table().Centered();
            table.BorderColor(Color.BlueViolet);
            table.Border(TableBorder.DoubleEdge);
            table.Expand();

            // Columns
            table.AddColumn("Setting Key");
            table.AddColumn("Value");

            // Column alignment
            table.Columns[0].RightAligned();
            table.Columns[1].RightAligned();

            bool isDebug = false;
            bool showHidden = false;

            PropertyInfo[] properties = settings.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(settings);
                if (property.Name == "Debug")
                    isDebug = (bool)(value ?? false);
                if (property.Name == "ShowHidden")
                    showHidden = (bool)(value ?? false);
            }
            if (isDebug)
            {
                if (showHidden)
                {
                    //if (connectionString != String.Empty)
                        //table.AddRow($"Database Connection:", $"{connectionString}");

                    table.AddRow($"ApiKey:", $"{server.ApiKey}");
                    table.AddRow($"Public Key:", $"{server.PublicKey}");
                    table.AddRow($"Private Key:", $"{server.PrivateKey}");
                }
                foreach (PropertyInfo property in properties)
                {
                    object? value = property.GetValue(settings);

                    if (property.PropertyType == typeof(bool))
                        table.AddRow($"{property.Name}", $"[yellow]{(bool)(value ?? false)}[/]");
                    else
                        if (value is not null)
                        table.AddRow($"{property.Name}", $"{value}");
                }
                AnsiConsole.Write(table);
                if (AnsiConsole.Confirm("Continue?", false))
                    Title.Print();
                else
                    return false;

            }
            return true;
        }
    }
}
