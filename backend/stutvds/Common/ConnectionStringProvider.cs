using System;
using Microsoft.Extensions.Configuration;

public class ConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public ConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString(string name = "Database")
    {
        var conn = _configuration.GetConnectionString(name);
        if (!string.IsNullOrWhiteSpace(conn))
            return conn;

        var envVar = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrWhiteSpace(envVar))
            return envVar;

        throw new InvalidOperationException($"Connection string '{name}' not found");
    }
}