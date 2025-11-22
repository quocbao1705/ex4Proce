using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ex4ProceDTO; 

namespace ex4ProceDAO
{
    public class NhaCungCapDAO
    {
        DBConnection db = new DBConnection();

        public List<NhaCungCapDTO> LayDanhSach()
        {
            List<NhaCungCapDTO> list = new List<NhaCungCapDTO>();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetAllNhaCungCap", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new NhaCungCapDTO
                    {
                        MaNCC = rd["MaNCC"].ToString(),
                        TenNCC = rd["TenNCC"].ToString()
                    });
                }
            }
            return list;
        }
    }
}