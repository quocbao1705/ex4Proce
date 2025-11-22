using System.Data.SqlClient;

namespace ex4ProceDAO
{
    public class DBConnection
    {
        private string strConn = @"Data Source=DESKTOP-GMTDUQN;Initial Catalog=PhieuNhapHang;Integrated Security=True   ";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(strConn);
        }
    }
}