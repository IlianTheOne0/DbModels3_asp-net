namespace WebApplication1.Features.Clients.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Client
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Surname { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string? Patronymic { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    public Address? Address { get; set; }
    public ICollection<ClientFinanceAccount> ClientFinanceAccounts { get; set; } = new List<ClientFinanceAccount>();
}