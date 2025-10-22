using AE.Ships.Domain.Entities;
using AE.Ships.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AE.Ships.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = new List<User>();
        
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetAllUsers", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                UserId = reader.GetInt32("UserId"),
                Name = reader.GetString("Name"),
                Role = reader.GetString("Role")
            });
        }
        
        return users;
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_GetUserById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", userId);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                UserId = reader.GetInt32("UserId"),
                Name = reader.GetString("Name"),
                Role = reader.GetString("Role")
            };
        }
        
        return null;
    }

    public async Task<User> CreateAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_CreateUser", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Role", user.Role);
        
        var userIdParam = new SqlParameter("@UserId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(userIdParam);
        
        await command.ExecuteNonQueryAsync();
        
        user.UserId = (int)userIdParam.Value;
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_UpdateUser", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", user.UserId);
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Role", user.Role);
        
        await command.ExecuteNonQueryAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_DeleteUser", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", userId);
        
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> ExistsAsync(int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("sp_UserExists", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", userId);
        
        var result = await command.ExecuteScalarAsync();
        return Convert.ToBoolean(result);
    }
}
