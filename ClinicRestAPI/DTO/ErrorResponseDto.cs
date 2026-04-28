using System.ComponentModel.DataAnnotations;
namespace ClinicRestAPI.DTO;

public class ErrorResponseDto
{
    [Required]
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
}