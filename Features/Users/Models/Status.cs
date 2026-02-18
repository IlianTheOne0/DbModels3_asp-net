namespace WebApplication1.Features.Users.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Status
{
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}