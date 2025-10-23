using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;

namespace AE.Ships.Infrastructure.Data;

public class UserShipAssignmentRepository : IUserShipAssignmentRepository
{
    private readonly string _connectionString;

    public UserShipAssignmentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<UserShipAssignmentDto> AssignShipToUserAsync(int userId, string shipCode)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);
        parameters.Add("@ShipCode", shipCode);

        var result = await connection.QueryFirstOrDefaultAsync<UserShipAssignmentDto>(
            "sp_AssignShipToUser", 
            parameters, 
            commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            throw new InvalidOperationException("Failed to assign ship to user.");
        }

        return result;
    }

    public async Task<bool> UnassignShipFromUserAsync(int userId, string shipCode)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);
        parameters.Add("@ShipCode", shipCode);

        var rowsAffected = await connection.QuerySingleAsync<int>(
            "sp_UnassignShipFromUser", 
            parameters, 
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<UserShipAssignmentDto>> GetUserShipAssignmentsAsync(int? userId = null, string? shipCode = null)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);
        parameters.Add("@ShipCode", shipCode);

        return await connection.QueryAsync<UserShipAssignmentDto>(
            "sp_GetUserShipAssignments", 
            parameters, 
            commandType: CommandType.StoredProcedure);
    }
}
