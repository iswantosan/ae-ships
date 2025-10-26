using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;

namespace AE.Ships.Infrastructure.Data;

public class FinancialReportRepository : IFinancialReportRepository
{
    private readonly string _connectionString;

    public FinancialReportRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<FinancialReportDto>> GetFinancialReportAsync(string shipCode, DateTime accountPeriod)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@ShipCode", shipCode);
        parameters.Add("@AccountPeriod", accountPeriod);

        return await connection.QueryAsync<FinancialReportDto>(
            "sp_GetFinancialReport", 
            parameters, 
            commandType: CommandType.StoredProcedure);
    }
}
