namespace ServiceCommon.Domain.Entities;

public class LogEntry
{
    public DateTime CreatedAt { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string ClientIdentifier { get; set; } = string.Empty;
}
