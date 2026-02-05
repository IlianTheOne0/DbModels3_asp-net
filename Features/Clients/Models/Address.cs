namespace WebApplication1.Features.Clients.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Address
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Region { get; set; } = null!;

    [MaxLength(100)]
    public string? Area { get; set; }

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Street { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Building { get; set; } = null!;

    [MaxLength(20)]
    public string? Apartment { get; set; }

    [MaxLength(10)]
    public string? Entrance { get; set; }

    [MaxLength(20)]
    public string? Room { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;
}