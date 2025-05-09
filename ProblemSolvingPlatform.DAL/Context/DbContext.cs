using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProblemSolvingPlatform.DAL.Context;

public class DbContext
{
    private readonly string _dbConnectionString;
    public DbContext(IConfiguration configuration)
    {
        _dbConnectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
    }
    public SqlConnection GetConnection()
    {
        return new SqlConnection(_dbConnectionString);
    }
}
