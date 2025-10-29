namespace ServiceMembership.Domain.Entities;

public class DetailsUser
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModification { get; set; }
    public bool IsActive { get; set; } = true;
    public int? IdUser { get; set; }
    public short? IdMembership { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public short? SessionsLeft { get; set; }
}
