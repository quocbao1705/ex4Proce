using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ex4ProceDTO;

namespace ex4ProceDAO
{
    public class PhieuNhapDAO
    {
        DBConnection db = new DBConnection();

        public List<PhieuNhapDTO> LayDanhSach()
        {
            List<PhieuNhapDTO> list = new List<PhieuNhapDTO>();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetAllPhieuNhap", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new PhieuNhapDTO
                    {
                        MaPN = rd["MaPN"].ToString(),
                        MaNCC = rd["MaNCC"].ToString(),
                        NgayNhap = DateTime.Parse(rd["NgayNhap"].ToString())
                    });
                }
            }
            return list;
        }

        public bool Them(PhieuNhapDTO pn)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertPhieuNhap", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaPN", pn.MaPN);
                    cmd.Parameters.AddWithValue("@MaNCC", pn.MaNCC);
                    cmd.Parameters.AddWithValue("@NgayNhap", pn.NgayNhap);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}