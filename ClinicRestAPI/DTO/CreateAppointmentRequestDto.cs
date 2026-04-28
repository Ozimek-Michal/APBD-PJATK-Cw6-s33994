using System.ComponentModel.DataAnnotations;

namespace ClinicRestAPI.DTO;

public class CreateAppointmentRequestDto
{
    [Required]
    public int DoctorId { get; set; }
    [Required]
    public int PatientId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required, MaxLength(250)]
    public string Reason { get; set; } = string.Empty;
}