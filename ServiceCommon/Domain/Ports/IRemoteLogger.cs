namespace ServiceCommon.Domain.Ports;

public interface IRemoteLogger
{
    Task LogInfo(string message, string user);
    Task LogWarning(string message, string user);
    Task LogError(string message, string user, Exception? ex = null);
}
