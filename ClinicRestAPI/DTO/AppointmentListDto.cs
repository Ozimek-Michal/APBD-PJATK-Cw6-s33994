using System.ComponentModel.DataAnnotations;

namespace ClinicRestAPI.DTO;

public class AppointmentListDto
{
    [Required]
    public int AppointmentId { get; set; }
    [Required]
    public string PatientFullName { get; set; } = string.Empty;
    [Required]
    public DateTime AppointmentDate { get; set; }
    [Required]
    public string Status { get; set; } = string.Empty;
    [Required, MaxLength(250)]
    public string Reason { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    
}