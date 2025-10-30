namespace ServiceCommon.Domain.Ports;

public interface IConnectionStringProvider
{
    string GetPostgresConnection();
}
