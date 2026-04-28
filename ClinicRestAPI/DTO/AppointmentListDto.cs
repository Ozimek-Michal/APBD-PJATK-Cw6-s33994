using System.ComponentModel.DataAnnotations;

namespace ClinicRestAPI.DTO;

public class AppointmentListDto
{
    [Required]
    public string PatientFullName { get; set; } = string.Empty;
    public List<AppointmentDetailsDto> AppointmentsList { get; set; } = [];
    
}