﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RobinhoodAccountConsole.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobinhoodAccountConsole;
public class Program
{
    public static async Task Main(string[] args)
    {
        // Configuration
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();

        IServiceCollection serviceCollection = ConfigureServices(config);
        var registrar = new TypeRegistrar(serviceCollection);

        var app = new CommandApp(registrar);
        app.Configure(configure => CommandApplication.Initialize(app));

        try
        {
            await app.RunAsync(args);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
        }

     }
    public static IServiceCollection ConfigureServices(IConfiguration config)
    {
        var database = config.GetSection("ConnectionStrings");
        var apiServer = config.GetSection("ApiServer");
        var services = new ServiceCollection();
        _ = services.AddSingleton(new ApiServer()
        {
            ApiKey = apiServer["ApiKey"] ?? throw new ArgumentNullException("ApiKey"),
            PublicKey = apiServer["PublicKey"] ?? throw new ArgumentNullException("PublicKey"),
            PrivateKey = apiServer["PrivateKey"] ?? throw new ArgumentNullException("PrivateKey")
        });
        services.AddSingleton(new ConnectionStrings() { DefaultDB = database["DefaultDB"] ?? throw new ArgumentNullException("DefaultDB") });
        services.BuildServiceProvider();
        return services;
    }
}
