using System.ComponentModel.DataAnnotations;

namespace ClinicRestAPI.DTO;

public class UpdateAppointmentRequestDto
{
    [Required]
    public int patientId { get; set; }
    [Required]
    public int doctorId { get; set; }
    [Required]
    public DateTime date { get; set; }
    [Required]
    public string status { get; set; } =  "Scheduled";
    [Required, MaxLength(250)]
    public string reason { get; set; } =  string.Empty;
    [MaxLength(500)]
    public string? internalNotes { get; set; }
}