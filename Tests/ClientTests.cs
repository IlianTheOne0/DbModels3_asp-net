namespace WebApplication1.Tests;

using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Data;
using WebApplication1.Features.Clients.Models;
using Xunit;

public class ClientTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateClient()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        context.Clients.Add(client);
        context.SaveChanges();
        Assert.Equal(1, context.Clients.Count());

        var existingClient = context.Clients.FirstOrDefault();
        Assert.NotNull(existingClient);
        Assert.Equal("Surname", existingClient.Surname);
        Assert.Equal("FirstName", existingClient.FirstName);
        Assert.Equal("Email", existingClient.Email);
        Assert.Equal("Patronymic", existingClient.Patronymic);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), existingClient.BirthDate);
    }

    [Fact]
    public void UpdateClient()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        context.Clients.Add(client);
        context.SaveChanges();
        client.Surname = "UpdatedSurname";
        client.FirstName = "UpdatedFirstName";

        context.Clients.Update(client);
        context.SaveChanges();

        var updateClient = context.Clients.First(client => client.Email == "Email");
        Assert.Equal("UpdatedSurname", updateClient.Surname);
        Assert.Equal("UpdatedFirstName", updateClient.FirstName);
    }

    [Fact]
    public void DeleteClient()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        context.Clients.Add(client);
        context.SaveChanges();

        Assert.Equal(1, context.Clients.Count());

        context.Clients.Remove(client);
        context.SaveChanges();

        Assert.Empty(context.Clients.ToList());

        var deletedClient = context.Clients.FirstOrDefault(client => client.Email == "Email");
        Assert.Null(deletedClient);
    }

    [Fact]
    public void GetClients()
    {
        using var context = GetDbContext();

        var clients = new List<Client>
        {
            new Client
            {
                Surname = "Surname1",
                FirstName = "FirstName1",
                Email = "Email1",
                Patronymic = "Patronymic1",
                BirthDate = DateOnly.FromDateTime(DateTime.Now)
            },
            new Client
            {
                Surname = "Surname2",
                FirstName = "FirstName2",
                Email = "Email2",
                Patronymic = "Patronymic2",
                BirthDate = DateOnly.FromDateTime(DateTime.Now)
            }
        };

        context.Clients.AddRange(clients);
        context.SaveChanges();

        var allClients = context.Clients.ToList();
        Assert.Equal(2, allClients.Count);

        Assert.Equal(clients[0], allClients[0]);
        Assert.Equal(clients[1], allClients[1]);

        Assert.Equal(clients[0].Surname, allClients[0].Surname);
    }

    [Fact]
    public void CreateClient_WithPhones()
    {
        using var context = GetDbContext();

        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now),
            Phones = new List<Phone>()
        };

        var phone1 = new Phone
        {
            Number = "123456789",
            Client = client
        };
        var phone2 = new Phone
        {
            Number = "987654321",
            Client = client
        };

        client.Phones.Add(phone1);
        client.Phones.Add(phone2);

        context.Clients.Add(client);
        context.SaveChanges();

        var savedClient = context.Clients.Include(client => client.Phones).FirstOrDefault();
        Assert.NotNull(savedClient);
        Assert.NotNull(savedClient.Phones);
        Assert.NotEmpty(savedClient.Phones);
        Assert.Equal(2, savedClient.Phones.Count);
        Assert.Equal("123456789", savedClient.Phones.First().Number);
    }
}