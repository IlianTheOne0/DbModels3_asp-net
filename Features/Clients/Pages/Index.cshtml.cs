namespace WebApplication1.Features.Clients.Pages;

using WebApplication1.Data;
using WebApplication1.Features.Clients.ViewModels;
using WebApplication1.Features.Clients.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class IndexModel : PageModel
{
    private readonly AppDbContext _dbContext;

    [BindProperty]
    public ClientViewModels Client { get; set; } = new ClientViewModels();
    public List<Client> Clients { get; set; } = new List<Client>();

    public IndexModel(AppDbContext dbContext) => _dbContext = dbContext;

    public void OnGet()
    {
        Clients = _dbContext.Clients.OrderByDescending(client => client.CreatedAt).ToList();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Clients = _dbContext.Clients.OrderByDescending(client => client.CreatedAt).ToList();
            return Page();
        }

        var newClient = new Client
        {
            Id = Guid.NewGuid(),
            Surname = Client.Surname,
            FirstName = Client.FirstName,
            Patronymic = Client.Patronymic,
            Email = Client.Email,
            BirthDate = Client.BirthDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Clients.Add(newClient);
        _dbContext.SaveChanges();

        return RedirectToPage("./Index");
    }
}