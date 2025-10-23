using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AE.Ships.Infrastructure.Data;

public class CrewRepository : ICrewRepository
{
    private readonly string _connectionString;

    public CrewRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<CrewListDto>> GetCrewListAsync(CrewListRequestDto request)
    {
        var crewList = new List<CrewListDto>();
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetCrewList", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        command.Parameters.AddWithValue("@ShipCode", request.ShipCode);
        command.Parameters.AddWithValue("@PageNumber", request.PageNumber);
        command.Parameters.AddWithValue("@PageSize", request.PageSize);
        command.Parameters.AddWithValue("@SortColumn", request.SortColumn);
        command.Parameters.AddWithValue("@SortDirection", request.SortDirection);
        command.Parameters.AddWithValue("@SearchTerm", request.SearchTerm ?? (object)DBNull.Value);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            crewList.Add(new CrewListDto
            {
                RankName = reader.GetString("Rank Name"),
                CrewMemberId = reader.GetString("Crew Member ID"),
                FirstName = reader.GetString("First Name"),
                LastName = reader.GetString("Last Name"),
                Age = reader.GetInt32("Age"),
                Nationality = reader.GetString("Nationality"),
                SignOnDate = reader.GetString("SignOnDate"),
                Status = reader.GetString("Status")
            });
        }
        
        return crewList;
    }

    public async Task<IEnumerable<CrewMemberHistoryDto>> GetCrewMemberHistoryAsync(string crewMemberId)
    {
        var history = new List<CrewMemberHistoryDto>();
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetCrewMemberHistory", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@CrewMemberId", crewMemberId);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            history.Add(new CrewMemberHistoryDto
            {
                CrewMemberId = reader.GetString("CrewMemberId"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                BirthDate = reader.GetDateTime("BirthDate"),
                Nationality = reader.GetString("Nationality"),
                ShipCode = reader.GetString("ShipCode"),
                ShipName = reader.GetString("ShipName"),
                RankName = reader.GetString("RankName"),
                SignOnDate = reader.GetDateTime("SignOnDate"),
                SignOffDate = reader.IsDBNull("SignOffDate") ? null : reader.GetDateTime("SignOffDate"),
                EndOfContractDate = reader.GetDateTime("EndOfContractDate"),
                Status = reader.GetString("Status")
            });
        }
        
        return history;
    }
}
