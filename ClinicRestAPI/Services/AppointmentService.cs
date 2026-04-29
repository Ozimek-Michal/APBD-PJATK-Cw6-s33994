using ClinicRestAPI.DTO;
using ClinicRestAPI.Exceptions;

namespace ClinicRestAPI.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

public class AppointmentService(IConfiguration configuration) : IAppointmentService
{
    private readonly string[] _allowedStatuses = ["Scheduled", "Cancelled", "Completed"];
    public async Task<List<AppointmentListDto>> GetAppointmentListAsync(string? status, string? lastName) 
    {
        if (!string.IsNullOrWhiteSpace(status) && !_allowedStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
        {
            throw new BadRequestException($"Status {status} is not allowed. Please choose beetwen Scheduled, Cancelled or Completed");
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            await using var c = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            await using var com = new SqlCommand();
            
            await c.OpenAsync();
            com.Connection = c;

            com.CommandText =
                """Select count(1) From Patients p JOIN Appointments a ON p.IdPatient = a.IdPatient WHERE p.LastName = @LN""";
            com.Parameters.AddWithValue("@LN", lastName);


            int count = (int)(await com.ExecuteScalarAsync() ?? 0);
            if (count == 0)
            {
                throw new NotFoundException($"Patient {lastName} has no recorded appointments");
            }
            
        }
        
        
        var appointmentlist = new List<AppointmentListDto>();

        await using var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        await using var command = new SqlCommand();

        await connection.OpenAsync();

        command.Connection = connection;

        command.CommandText =
            """
            SELECT a.IdAppointment, a.AppointmentDate, a.Status, 
            a.Reason,p.FirstName + N' ' + p.LastName AS PatientFullName,
            p.Email AS PatientEmail FROM Appointments a
            JOIN Patients p ON p.IdPatient = a.IdPatient
            WHERE(@Status IS NULL OR a.Status = @Status)
            AND(@PatientLastName IS NULL OR p.LastName = @PatientLastName)
            ORDER BY a.AppointmentDate
            """;
        command.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);
        command.Parameters.AddWithValue("@PatientLastName", (object?)lastName ?? DBNull.Value);
        
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            appointmentlist.Add(new AppointmentListDto()
            {
                AppointmentId = reader.GetInt32(0),
                AppointmentDate = reader.GetDateTime(1),
                Status = reader.GetString(2),
                Reason = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                PatientFullName = reader.GetString(4),
                Email = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)

            });
        }

        return appointmentlist;
    }
}