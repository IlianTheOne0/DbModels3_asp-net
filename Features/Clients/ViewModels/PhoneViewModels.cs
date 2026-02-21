using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Features.Clients.ViewModels;

public class PhoneViewModels
{
    [Required]
    [StringLength(50)]
    public string Number { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
}