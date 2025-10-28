namespace ServiceCommon.Domain.Entities;

public class LogEntry
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Level { get; set; }
    public string? Message { get; set; }
    public string? ClientIdentifier { get; set; }
}