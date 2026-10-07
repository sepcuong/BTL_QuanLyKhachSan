using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DAL.Helper.Interfaces;

namespace DAL.Helper
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            // Đọc chuỗi kết nối từ appsettings.json
            _connectionString = configuration["ConnectionStrings:Cuongdz"];
        }

        // Dapper cần một IDbConnection để hoạt động
        public IDbConnection TConnection()
        {
            var connection = new SqlConnection(_connectionString);
            // Mở sẵn kết nối để các Repository có thể dùng ngay
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }
    }
}