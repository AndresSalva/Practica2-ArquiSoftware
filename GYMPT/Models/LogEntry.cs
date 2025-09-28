using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace GYMPT.Models
{
    [Table("logs")]
    public class LogEntry : BaseModel
    {
        [PrimaryKey("id")]
        public long Id { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("level")]
        public string Level { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("client_identifier")]
        public string ClientIdentifier { get; set; }
    }
}