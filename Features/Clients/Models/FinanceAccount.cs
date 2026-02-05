using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Features.Clients.Models;

public class FinanceAccount
{
    public Guid Id { get; set; }

    public decimal Balance { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<ClientFinanceAccount> ClientFinanceAccounts { get; set; } = new List<ClientFinanceAccount>();
}