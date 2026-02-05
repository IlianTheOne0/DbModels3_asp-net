namespace WebApplication1.Features.Users.Models;

using System.ComponentModel.DataAnnotations;

public class Status
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}