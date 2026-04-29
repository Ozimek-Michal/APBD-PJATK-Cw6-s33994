using Microsoft.AspNetCore.Mvc;
using ClinicRestAPI.DTO;
using ClinicRestAPI.Exceptions;
using ClinicRestAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ClinicRestAPI.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController(IAppointmentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAppointmentList(string? status, string? patientLastName)
    {
        try
        {
            List<AppointmentListDto> appointments = await service.GetAppointmentListAsync(status, patientLastName);
            return Ok(appointments);
        }
        catch (Exception e)
        {
            var errordto = new ErrorResponseDto(){Message = "Appointment list not found", Details =  e.Message};
            if (e is BadRequestException)
                return BadRequest(errordto);
            return NotFound(errordto);
        }
        
        
    }
}