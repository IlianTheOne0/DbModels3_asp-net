namespace WebApplication1Tests;

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Features.Clients.Models;
using Xunit;

public class FinanceAccountTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateFinanceAccount()
    {
        using var context = GetDbContext();

        var account = new FinanceAccount { Balance = 1500.50m };

        context.FinanceAccounts.Add(account);
        context.SaveChanges();

        var savedAccount = context.FinanceAccounts.FirstOrDefault();
        Assert.NotNull(savedAccount);
        Assert.Equal(1500.50m, savedAccount.Balance);
        Assert.NotEqual(default, savedAccount.CreatedAt);
    }

    [Fact]
    public void UpdateFinanceAccountBalance()
    {
        using var context = GetDbContext();

        var account = new FinanceAccount { Balance = 100.00m };
        context.FinanceAccounts.Add(account);
        context.SaveChanges();

        account.Balance = 250.75m;
        context.FinanceAccounts.Update(account);
        context.SaveChanges();

        var updatedAccount = context.FinanceAccounts.First();
        Assert.Equal(250.75m, updatedAccount.Balance);
    }

    [Fact]
    public void AssignFinanceAccount_ToClient_ManyToMany()
    {
        using var context = GetDbContext();

        var client = new Client
        {
            Surname = "Wayne",
            FirstName = "Bruce",
            Email = "bruce@wayneenterprises.com"
        };

        var account = new FinanceAccount { Balance = 1000000.00m };

        context.Clients.Add(client);
        context.FinanceAccounts.Add(account);
        context.SaveChanges();

        var clientFinanceAccount = new ClientFinanceAccount
        {
            ClientId = client.Id,
            FinanceAccountId = account.Id
        };

        context.ClientFinanceAccounts.Add(clientFinanceAccount);
        context.SaveChanges();

        var savedClient = context.Clients
            .Include(client => client.ClientFinanceAccounts)
            .ThenInclude(clientFinanceAccount => clientFinanceAccount.FinanceAccount)
            .FirstOrDefault(client => client.Email == "bruce@wayneenterprises.com");

        Assert.NotNull(savedClient);
        Assert.Single(savedClient.ClientFinanceAccounts);
        Assert.Equal(1000000.00m, savedClient.ClientFinanceAccounts.First().FinanceAccount.Balance);
    }

    [Fact]
    public void DeleteFinanceAccount_RemovesMappingButKeepsClient()
    {
        using var context = GetDbContext();

        var client = new Client
        {
            Surname = "Kent",
            FirstName = "Clark",
            Email = "clark@dailyplanet.com"
        };
        var account = new FinanceAccount { Balance = 50.00m };

        context.Clients.Add(client);
        context.FinanceAccounts.Add(account);
        context.SaveChanges();

        var mapping = new ClientFinanceAccount
        {
            ClientId = client.Id,
            FinanceAccountId = account.Id
        };

        context.ClientFinanceAccounts.Add(mapping);
        context.SaveChanges();

        context.FinanceAccounts.Remove(account);
        context.SaveChanges();

        Assert.Empty(context.FinanceAccounts.ToList());
        Assert.Empty(context.ClientFinanceAccounts.ToList());
        Assert.NotNull(context.Clients.FirstOrDefault(client => client.Email == "clark@dailyplanet.com"));
    }
}