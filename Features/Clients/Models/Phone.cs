namespace WebApplication1.Features.Clients.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Phone
{
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Number { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;
}

public enum CountryCode
{
    UA = 300,
    US = 1,
    GB = 44,
    DE = 49,
    FR = 33,
    IT = 39,
    ES = 34,
    CA = 1,
    AU = 61,
    IN = 91
}