namespace WebApplication1.Features.Users.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }

    public Guid StatusId { get; set; }
    public Status Status { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}