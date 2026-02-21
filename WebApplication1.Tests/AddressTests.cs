namespace WebApplication1Tests;

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Features.Clients.Models;
using Xunit;

public class AddressTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateAddress_ForClient_OneToOne()
    {
        using var context = GetDbContext();

        var client = new Client
        {
            Surname = "Doe",
            FirstName = "John",
            Email = "john.doe@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var address = new Address
        {
            Country = "Ukraine",
            Region = "Vinnytsia Oblast",
            City = "Vinnytsia",
            Street = "Soborna",
            Building = "10",
            Client = client
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        var savedAddress = context.Addresses.Include(address => address.Client).FirstOrDefault();

        Assert.NotNull(savedAddress);
        Assert.Equal("Ukraine", savedAddress.Country);
        Assert.NotNull(savedAddress.Client);
        Assert.Equal("john.doe@test.com", savedAddress.Client.Email);
    }

    [Fact]
    public void UpdateAddress()
    {
        using var context = GetDbContext();

        var client = new Client
        {
            Surname = "Smith",
            FirstName = "Jane",
            Email = "jane.smith@test.com"
        };

        var address = new Address
        {
            Country = "USA",
            Region = "NY",
            City = "New York",
            Street = "Broadway",
            Building = "1",
            Client = client
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        address.City = "Albany";
        address.Building = "5A";
        context.Addresses.Update(address);
        context.SaveChanges();

        var updatedAddress = context.Addresses.First();
        Assert.Equal("Albany", updatedAddress.City);
        Assert.Equal("5A", updatedAddress.Building);
    }

    [Fact]
    public void DeleteAddress_KeepsClient()
    {
        using var context = GetDbContext();

        var client = new Client
        {
            Surname = "Brown",
            FirstName = "Doc",
            Email = "doc@test.com"
        };

        var address = new Address
        {
            Country = "USA",
            Region = "CA",
            City = "Hill Valley",
            Street = "Riverside Drive",
            Building = "1640",
            Client = client
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        context.Addresses.Remove(address);
        context.SaveChanges();

        Assert.Empty(context.Addresses.ToList());

        var savedClient = context.Clients.FirstOrDefault(client => client.Email == "doc@test.com");
        Assert.NotNull(savedClient);
    }
}