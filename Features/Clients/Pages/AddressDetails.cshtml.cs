namespace WebApplication1.Features.Clients.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Features.Clients.Models;
using WebApplication1.Features.Clients.ViewModels;

public class AddressDetailsModel : PageModel
{
    private readonly AppDbContext _dbContext;

    [BindProperty]
    public AddressViewModels AddressInfo { get; set; } = new AddressViewModels();

    public string ClientName { get; set; } = string.Empty;

    public bool HasAddress { get; set; }

    public AddressDetailsModel(AppDbContext dbContext) => _dbContext = dbContext;

    public IActionResult OnGet(Guid id)
    {
        var client = _dbContext.Clients
            .Include(client => client.Address)
            .FirstOrDefault(client => client.Id == id);

        if (client == null) { return NotFound(); }

        ClientName = $"{client.FirstName} {client.Surname}";

        if (client.Address != null)
        {
            HasAddress = true;

            AddressInfo = new AddressViewModels
            {
                Country = client.Address.Country,
                Region = client.Address.Region,
                Area = client.Address.Area,
                City = client.Address.City,
                Street = client.Address.Street,
                Building = client.Address.Building,
                Apartment = client.Address.Apartment,
                Entrance = client.Address.Entrance,
                Room = client.Address.Room
            };
        }

        return Page();
    }

    public IActionResult OnPost(Guid id)
    {
        if (!ModelState.IsValid) { return Page(); }

        var client = _dbContext.Clients
            .Include(client => client.Address)
            .FirstOrDefault(client => client.Id == id);

        if (client == null) { return NotFound(); }

        if (client.Address == null)
        {
            client.Address = new Address
            {
                Id = Guid.NewGuid(),
                ClientId = id,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Addresses.Add(client.Address);
        }

        client.Address.Country = AddressInfo.Country;
        client.Address.Region = AddressInfo.Region;
        client.Address.Area = AddressInfo.Area;
        client.Address.City = AddressInfo.City;
        client.Address.Street = AddressInfo.Street;
        client.Address.Building = AddressInfo.Building;
        client.Address.Apartment = AddressInfo.Apartment;
        client.Address.Entrance = AddressInfo.Entrance;
        client.Address.Room = AddressInfo.Room;
        client.Address.UpdatedAt = DateTime.UtcNow;

        _dbContext.SaveChanges();

        return RedirectToPage("./Index");
    }

    public IActionResult OnPostDelete(Guid id)
    {
        var client = _dbContext.Clients
            .Include(client => client.Address)
            .FirstOrDefault(client => client.Id == id);

        if (client == null) { return NotFound(); }

        if (client.Address != null)
        {
            _dbContext.Addresses.Remove(client.Address);
            _dbContext.SaveChanges();
        }

        return RedirectToPage("./Index");
    }
}