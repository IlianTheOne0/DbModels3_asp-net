namespace WebApplication1.Features.Clients.Models;

public class ClientFinanceAccount
{
    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public Guid FinanceAccountId { get; set; }
    public FinanceAccount FinanceAccount { get; set; } = null!;
}