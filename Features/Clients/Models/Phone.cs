namespace WebApplication1.Features.Clients.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Phone
{
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string Number { get; set; } = null!;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;
}