namespace WebApplication1.Data;

using WebApplication1.Features.Users.Models;
using WebApplication1.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<FinanceAccount> FinanceAccounts { get; set; }
    public DbSet<ClientFinanceAccount> ClientFinanceAccounts { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Status> Statuses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .HasIndex(client => client.Email)
            .IsUnique();

        modelBuilder.Entity<Client>()
            .HasOne(client => client.Address)
            .WithOne(client => client.Client)
            .HasForeignKey<Address>(client => client.ClientId);

        modelBuilder.Entity<Client>()
            .HasMany(client => client.Phones)
            .WithOne(client => client.Client)
            .HasForeignKey(client => client.ClientId);

        modelBuilder.Entity<ClientFinanceAccount>()
            .HasKey(client => new { client.ClientId, client.FinanceAccountId });

        modelBuilder.Entity<ClientFinanceAccount>()
            .HasOne(client => client.Client)
            .WithMany(client => client.ClientFinanceAccounts)
            .HasForeignKey(client => client.ClientId);

        modelBuilder.Entity<ClientFinanceAccount>()
            .HasOne(client => client.FinanceAccount)
            .WithMany(client => client.ClientFinanceAccounts)
            .HasForeignKey(client => client.FinanceAccountId);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Status)
            .WithMany(user => user.Users)
            .HasForeignKey(user => user.StatusId);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Role)
            .WithMany(user => user.Users)
            .HasForeignKey(user => user.RoleId);
    }
}