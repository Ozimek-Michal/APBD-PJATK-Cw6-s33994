using ClinicRestAPI.DTO;

namespace ClinicRestAPI.Services;

public interface IAppointmentService
{
    Task<List<AppointmentListDto>> GetAppointmentListAsync(string status, string lastName);

}