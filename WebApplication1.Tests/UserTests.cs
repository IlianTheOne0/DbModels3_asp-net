namespace WebApplication1Tests;

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using WebApplication1.Data;
using WebApplication1.Features.Users.Models;
using Xunit;

public class UserTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateUser_WithRoleAndStatus()
    {
        using var context = GetDbContext();

        var role = new Role { Name = "Admin" };
        var status = new Status { Name = "Active" };

        var user = new User
        {
            Login = "testuser",
            Password = "hashedpassword",
            Email = "test@user.com",
            Role = role,
            Status = status
        };

        context.Users.Add(user);
        context.SaveChanges();

        var savedUser = context.Users.Include(user => user.Role).Include(user => user.Status).FirstOrDefault();

        Assert.NotNull(savedUser);
        Assert.Equal("testuser", savedUser.Login);
        Assert.Equal("Admin", savedUser.Role.Name);
        Assert.Equal("Active", savedUser.Status.Name);
    }

    [Fact]
    public void UpdateUser_ChangesEmailAndStatus()
    {
        using var context = GetDbContext();

        var role = new Role { Id = Guid.NewGuid(), Name = "StandardUser" };
        var activeStatus = new Status { Id = Guid.NewGuid(), Name = "Active" };
        var bannedStatus = new Status { Id = Guid.NewGuid(), Name = "Banned" };

        context.Roles.Add(role);
        context.Statuses.Add(activeStatus);
        context.Statuses.Add(bannedStatus);
        context.SaveChanges();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "johndoe",
            Password = "securepassword",
            Email = "john.old@example.com",
            RoleId = role.Id,
            StatusId = activeStatus.Id
        };

        context.Users.Add(user);
        context.SaveChanges();

        user.Email = "john.new@example.com";
        user.StatusId = bannedStatus.Id;
        user.UpdatedAt = DateTime.UtcNow;

        context.Users.Update(user);
        context.SaveChanges();

        var updatedUser = context.Users.Include(user => user.Status).FirstOrDefault(user => user.Login == "johndoe");

        Assert.NotNull(updatedUser);
        Assert.Equal("john.new@example.com", updatedUser.Email);
        Assert.Equal("Banned", updatedUser.Status.Name);
    }

    [Fact]
    public void DeleteUser()
    {
        using var context = GetDbContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "deleteme",
            Password = "password123",
            Email = "delete@example.com",
            Role = new Role { Name = "Guest" },
            Status = new Status { Name = "Pending" }
        };

        context.Users.Add(user);
        context.SaveChanges();

        Assert.Equal(1, context.Users.Count());

        context.Users.Remove(user);
        context.SaveChanges();

        Assert.Empty(context.Users.ToList());

        var deletedUser = context.Users.FirstOrDefault(user => user.Email == "delete@example.com");
        Assert.Null(deletedUser);
    }

    [Fact]
    public void GetUsers_BySpecificRole()
    {
        using var context = GetDbContext();

        var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin" };
        var userRole = new Role { Id = Guid.NewGuid(), Name = "User" };
        var status = new Status { Id = Guid.NewGuid(), Name = "Active" };

        context.Users.AddRange(new List<User>
        {
            new User { Login = "admin1", Password = "pw", Email = "a1@test.com", Role = adminRole, Status = status },
            new User { Login = "admin2", Password = "pw", Email = "a2@test.com", Role = adminRole, Status = status },
            new User { Login = "user1", Password = "pw", Email = "u1@test.com", Role = userRole, Status = status }
        });

        context.SaveChanges();

        var admins = context.Users
            .Include(user => user.Role)
            .Where(user => user.Role.Name == "Admin")
            .ToList();

        Assert.Equal(2, admins.Count);
        Assert.All(admins, user => Assert.Equal("Admin", user.Role.Name));
    }

    [Fact]
    public void CreateRole_AndAssignMultipleUsers()
    {
        using var context = GetDbContext();

        var role = new Role { Name = "Manager" };
        var status = new Status { Name = "Active" };

        var user1 = new User { Login = "mgr1", Password = "123", Email = "mgr1@test.com", Status = status };
        var user2 = new User { Login = "mgr2", Password = "123", Email = "mgr2@test.com", Status = status };

        role.Users.Add(user1);
        role.Users.Add(user2);

        context.Roles.Add(role);
        context.SaveChanges();

        var savedRole = context.Roles.Include(role => role.Users).FirstOrDefault(role => role.Name == "Manager");

        Assert.NotNull(savedRole);
        Assert.Equal(2, savedRole.Users.Count);
        Assert.Contains(savedRole.Users, user => user.Login == "mgr1");
        Assert.Contains(savedRole.Users, user => user.Login == "mgr2");
    }

    [Fact]
    public void CreateStatus_AndAssignMultipleUsers()
    {
        using var context = GetDbContext();

        var role = new Role { Name = "Tester" };
        var status = new Status { Name = "Suspended" };

        var user1 = new User { Login = "tst1", Password = "123", Email = "tst1@test.com", Role = role };
        var user2 = new User { Login = "tst2", Password = "123", Email = "tst2@test.com", Role = role };

        status.Users.Add(user1);
        status.Users.Add(user2);

        context.Statuses.Add(status);
        context.SaveChanges();

        var savedStatus = context.Statuses.Include(status => status.Users).FirstOrDefault(status => status.Name == "Suspended");

        Assert.NotNull(savedStatus);
        Assert.Equal(2, savedStatus.Users.Count);
    }

    [Fact]
    public void CheckUserTimestamps_DefaultAssignment()
    {
        using var context = GetDbContext();

        var role = new Role { Name = "User" };
        var status = new Status { Name = "Active" };

        var user = new User
        {
            Login = "timecheck",
            Password = "pwd",
            Email = "time@test.com",
            Role = role,
            Status = status
        };

        context.Users.Add(user);
        context.SaveChanges();

        var savedUser = context.Users.First();

        Assert.NotEqual(default, savedUser.CreatedAt);
        Assert.Equal(default, savedUser.UpdatedAt);
    }
}