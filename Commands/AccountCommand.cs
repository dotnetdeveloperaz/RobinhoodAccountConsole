
using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Cli;
using RobinhoodAccountConsole.Models;

namespace RobinhoodAccountConsole.Commands;

public class AccountCommand : AsyncCommand<AccountCommand.Settings>
{
    private readonly ApiServer _apiServer;
    private static string _url = "https://trading.robinhood.com/api/v1/crypto/trading/accounts/";

    public AccountCommand(ApiServer apiServer)
    {
        _apiServer = apiServer;
    }

    public class Settings : CommandSettings
    {

    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        string[] columns = new[] { "" };
        var table = new Table().Centered();
        table.HideHeaders();
        table.BorderColor(Color.Green3);
        table.Border(TableBorder.Rounded);
        table.AddColumns(columns);
        table.Expand();

        await AnsiConsole
            .Live(table)
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Top)
            .StartAsync(async ctx =>
            {
                void Update(int delay, Action action)
                {
                    action();
                    ctx.Refresh();
                    Thread.Sleep(delay);
                }

                //Update(70, () => table.Columns[0].Footer($"[green bold]Initializing Account Information[/]"));
                //var body = new { };
                string path = "/api/v1/crypto/trading/accounts/";
                string method = "GET";
                Signature signature = new Signature(_apiServer, method, path);
                Update(70, () => table.AddRow($"Creating Signature...."));
                signature.Sign();
                Update(70, () => table.AddRow($"Signature Is Valid: {signature.IsSignatureValid}"));

                if (signature.IsSignatureValid)
                {
                    Update(70, () => table.AddRow($"[green]Retrieving Account Information....[/]"));
                    Account? account = await GetAccountInformation(_apiServer, signature);
                    if (account != null)
                    {
                        Update(70, () => table.AddRow($"[green]Account Number: {account.AccountNumber} Status: {account.Status}[/]"));
                        Update(70, () => table.AddRow($"[green]Buying Power: ${account?.BuyingPower}      Currency: {account?.BuyingPowerCurrency}[/]"));
                    }
                }
                else
                    Update(70, () => table.AddRow($"[red bold]Could Not Retrieve Account Information.[/]"));
            });
        return 0;
    }

    private static async Task<Account?> GetAccountInformation(ApiServer apiServer, Signature signature)
    {
        Account? account = null;
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("x-api-key", apiServer.ApiKey);
            client.DefaultRequestHeaders.Add("x-timestamp", signature.Timestamp.ToString());
            client.DefaultRequestHeaders.Add("x-signature", signature.Base64Signature);
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _url))
            {
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStreamAsync();
                account = await JsonSerializer.DeserializeAsync<Account>(result);
            }
        }
        return account;
    }
}