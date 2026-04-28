using Microsoft.AspNetCore.Mvc;
using ClinicRestAPI.DTO;
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
        return Ok(await service.GetAppointmentListAsync(status, patientLastName));
    }
}