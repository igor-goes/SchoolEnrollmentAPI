using System.Configuration; using System.Data.SqlClient;
namespace SchoolEnrollmentAPI.Infrastructure { public class SqlConnectionFactory { public SqlConnection Create() { return new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolEnrollment"].ConnectionString); } } }
