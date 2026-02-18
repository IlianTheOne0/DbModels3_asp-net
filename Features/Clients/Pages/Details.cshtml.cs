namespace WebApplication1.Features.Clients.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;
using WebApplication1.Features.Clients.Models;
using WebApplication1.Features.Clients.ViewModels;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _dbContext;

    [BindProperty]
    public ClientViewModels Client { get; set; } = new ClientViewModels();

    public DetailsModel(AppDbContext dbContext) => _dbContext = dbContext;

    public void OnGet(Guid id)
    {
        Client = _dbContext.Clients
            .Where(client => client.Id == id)
            .Select
            (
                client => new ClientViewModels
                {
                    Surname = client.Surname,
                    FirstName = client.FirstName,
                    Patronymic = client.Patronymic,
                    Email = client.Email,
                    BirthDate = client.BirthDate
                }
            )
            .FirstOrDefault() ?? new ClientViewModels();
    }

    public IActionResult OnPost()
    {
        Console.WriteLine($"Received client data: {Client.FirstName} {Client.Surname}, Email: {Client.Email}, BirthDate: {Client.BirthDate}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model state is invalid. Returning to page with validation errors.");
            return Page();
        }

        Console.WriteLine("Model state is valid. Proceeding to create new client.");

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

        Console.WriteLine($"New client created with ID: {newClient.Id}");

        return RedirectToPage("./Index");
    }
}
