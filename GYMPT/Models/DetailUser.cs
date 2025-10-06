namespace GYMPT.Models
{
    public class DetailsUser
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModification { get; set; }
        public bool IsActive { get; set; } = true;
        public int? IdUser { get; set; }
        public short? IdMembership { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public short? SessionsLeft { get; set; }
    }
}