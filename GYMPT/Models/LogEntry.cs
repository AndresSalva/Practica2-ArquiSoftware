
namespace GYMPT.Models
{

    public class LogEntry
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string ClientIdentifier { get; set; }
    }
}