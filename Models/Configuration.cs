namespace RobinhoodCreateSignature.Models;

public class Configuration
{
    public required ApiServer ApiServer { get; set; }
    public required Logging Logging { get; set; }
    public required ConnectionStrings ConnectionStrings { get; set; }
    public string AllowedHosts { get; set; } = "";
}

public class ApiServer
{
    public string ApiKey { get; set; } = "";
    public string PublicKey { get; set; } = "";
    public string PrivateKey { get; set; } = "";
}

public class ConnectionStrings
{
    public string DefaultDB { get; set; } = "";
}

public class Logging
{
    public required LogLevel LogLevel { get; set; }
}

public class LogLevel
{
    public string Default { get; set; } = "";
    public string Microsoft { get; set; } = "";

    public string MicrosoftHostingLifetime { get; set; } = "";
}