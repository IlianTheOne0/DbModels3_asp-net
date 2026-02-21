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

    [BindProperty]
    public PhoneViewModels Phone { get; set; } = new PhoneViewModels();

    public List<Phone> Phones { get; set; } = new List<Phone>();

    [BindProperty]
    public bool ShowPhonesForm { get; set; }

    [BindProperty]
    public Guid ClientId { get; set; }

    public DetailsModel(AppDbContext dbContext) => _dbContext = dbContext;

    public void OnGet(Guid id)
    {
        ClientId = id;

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

        Phones = _dbContext.Phones
            .Where(phone => phone.ClientId == id)
            .ToList();
    }

    public IActionResult OnPost(Guid id)
    {
        ClientId = id;

        ModelState.Clear();
        TryValidateModel(Client, nameof(Client));

        if (!ModelState.IsValid)
        {
            Phones = _dbContext.Phones.Where(phone => phone.ClientId == id).ToList();
            return Page();
        }

        var existingClient = _dbContext.Clients.FirstOrDefault(client => client.Id == id);
        if (existingClient == null) { return NotFound(); }

        existingClient.Surname = Client.Surname;
        existingClient.FirstName = Client.FirstName;
        existingClient.Patronymic = Client.Patronymic;
        existingClient.Email = Client.Email;
        existingClient.BirthDate = Client.BirthDate;
        existingClient.UpdatedAt = DateTime.UtcNow;

        _dbContext.SaveChanges();

        return RedirectToPage("./Index");
    }

    public IActionResult OnPostShowPhonesForm(Guid id)
    {
        OnGet(id);
        ShowPhonesForm = true;
        return Page();
    }

    public IActionResult OnPostAddPhone(Guid id)
    {
        ModelState.Clear();

        TryValidateModel(Phone, nameof(Phone));

        if (!ModelState.IsValid)
        {
            OnGet(id);
            ShowPhonesForm = true;
            return Page();
        }

        string countryCodeText = Enum.TryParse<CountryCode>(Phone.CountryCode, out var parsedCode) ? parsedCode.ToString() : Phone.CountryCode;

        var newPhone = new Phone
        {
            Id = Guid.NewGuid(),
            Number = Phone.Number,
            CountryCode = countryCodeText,
            ClientId = id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Phones.Add(newPhone);
        _dbContext.SaveChanges();

        return RedirectToPage(new { id = id });
    }
}