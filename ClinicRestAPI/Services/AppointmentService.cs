using ClinicRestAPI.DTO;

namespace ClinicRestAPI.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

public class AppointmentService(IConfiguration configuration) : IAppointmentService
{
    public async Task<List<AppointmentListDto>> GetAppointmentListAsync(string? status, string? lastName)
    {
        var appointmentlist = new List<AppointmentListDto>();

        await using var connection = new SqlConnection(configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();

        await connection.OpenAsync();

        command.Connection = connection;

        command.CommandText =
            """
            SELECT a.IdAppointment, a.AppointmentDate, a.Status, 
            a.Reason,p.FirstName + N' ' + p.LastName AS PatientFullName,
            p.Email AS PatientEmail FROM dbo.Appointments a
            JOIN dbo.Patients p ON p.IdPatient = a.IdPatient
            WHERE(@Status IS NULL OR a.Status = @Status)
            AND(@PatientLastName IS NULL OR p.LastName = @PatientLastName)
            ORDER BY a.AppointmentDate
            """;
        command.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);
        command.Parameters.AddWithValue("@PatientLastName", (object?)lastName ?? DBNull.Value);
        
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        { 
            appointmentlist.Add( new AppointmentListDto()
            {
                AppointmentId = reader.GetInt32(0),
                AppointmentDate = reader.GetDateTime(1),
                Status = reader.GetString(2),
                Reason = reader.IsDBNull(3) ? string.Empty : reader.GetString(3), 
                PatientFullName = reader.GetString(4),
                Email = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            
        }
        
        return appointmentlist;
    }
}