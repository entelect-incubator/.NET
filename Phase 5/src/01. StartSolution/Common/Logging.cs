namespace Common;

using System;

public static class Logging
{
    public static void LogInfo(string name, object? data = null)
    {
        var payload = data is null ? name : $"{name} | Data: {data}";
        Console.WriteLine(payload);
    }

    public static void LogException(Exception exception)
    {
        Console.WriteLine(exception.ToString());
    }
}
