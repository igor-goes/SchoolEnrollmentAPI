using System.Configuration;
using System.Data.SqlClient;

namespace SchoolEnrollmentAPI.Infrastructure
{
    /// <summary>
    /// Cria conexões SQL Server a partir da connection string configurada para a API.
    /// </summary>
    public class SqlConnectionFactory
    {
        public SqlConnection Create()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolEnrollment"].ConnectionString);
        }
    }
}
