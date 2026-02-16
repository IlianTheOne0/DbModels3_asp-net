namespace WebApplication1.Features.Clients.ViewModels;

using System.ComponentModel.DataAnnotations;

public class ClientViewModels
{
    [Required]
    [StringLength(50)]
    public string Surname { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string? Patronymic { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public DateOnly BirthDate { get; set; }
}