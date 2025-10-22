using AE.Ships.Domain.Entities;
using AE.Ships.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AE.Ships.Infrastructure.Data;

public class ShipRepository : IShipRepository
{
    private readonly string _connectionString;

    public ShipRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Ship>> GetAllAsync()
    {
        var ships = new List<Ship>();
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetAllShips", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ships.Add(new Ship
            {
                Code = reader.GetString("Code"),
                Name = reader.GetString("Name"),
                FiscalYear = reader.GetString("FiscalYear"),
                Status = reader.GetString("Status")
            });
        }
        
        return ships;
    }

    public async Task<Ship?> GetByCodeAsync(string shipCode)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetShipByCode", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@ShipCode", shipCode);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Ship
            {
                Code = reader.GetString("Code"),
                Name = reader.GetString("Name"),
                FiscalYear = reader.GetString("FiscalYear"),
                Status = reader.GetString("Status")
            };
        }
        
        return null;
    }

    public async Task<Ship> CreateAsync(Ship ship)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_CreateShip", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Code", ship.Code);
        command.Parameters.AddWithValue("@Name", ship.Name);
        command.Parameters.AddWithValue("@FiscalYear", ship.FiscalYear);
        command.Parameters.AddWithValue("@Status", ship.Status);
        
        await command.ExecuteNonQueryAsync();
        return ship;
    }

    public async Task<Ship> UpdateAsync(Ship ship)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_UpdateShip", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Code", ship.Code);
        command.Parameters.AddWithValue("@Name", ship.Name);
        command.Parameters.AddWithValue("@FiscalYear", ship.FiscalYear);
        command.Parameters.AddWithValue("@Status", ship.Status);
        
        await command.ExecuteNonQueryAsync();
        return ship;
    }

    public async Task<bool> DeleteAsync(string shipCode)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_DeleteShip", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@ShipCode", shipCode);
        
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsAsync(string shipCode)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_ShipExists", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@ShipCode", shipCode);
        
        var result = await command.ExecuteScalarAsync();
        return Convert.ToBoolean(result);
    }

    public async Task<IEnumerable<Ship>> GetByStatusAsync(string status)
    {
        var ships = new List<Ship>();
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetShipsByStatus", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Status", status);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ships.Add(new Ship
            {
                Code = reader.GetString("Code"),
                Name = reader.GetString("Name"),
                FiscalYear = reader.GetString("FiscalYear"),
                Status = reader.GetString("Status")
            });
        }
        
        return ships;
    }

    public async Task<IEnumerable<Ship>> GetByUserAsync(int userId)
    {
        var ships = new List<Ship>();
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetShipsByUser", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", userId);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ships.Add(new Ship
            {
                Code = reader.GetString("Code"),
                Name = reader.GetString("Name"),
                FiscalYear = reader.GetString("FiscalYear"),
                Status = reader.GetString("Status")
            });
        }
        
        return ships;
    }
}

