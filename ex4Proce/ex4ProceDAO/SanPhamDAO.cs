using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ex4ProceDTO;

namespace ex4ProceDAO
{
    public class SanPhamDAO
    {
        DBConnection db = new DBConnection();

        public List<SanPhamDTO> LayDanhSach()
        {
            List<SanPhamDTO> list = new List<SanPhamDTO>();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetAllSanPham", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new SanPhamDTO
                    {
                        MaSP = rd["MaSP"].ToString(),
                        TenSP = rd["TenSP"].ToString(),
                        DonGia = rd["DonGia"] != DBNull.Value ? (decimal?)rd["DonGia"] : 0
                    });
                }
            }
            return list;
        }

        public SanPhamDTO LayTheoMa(string maSP)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetSanPhamByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSP", maSP);

                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    return new SanPhamDTO
                    {
                        MaSP = rd["MaSP"].ToString(),
                        TenSP = rd["TenSP"].ToString(),
                        DonGia = rd["DonGia"] != DBNull.Value ? (decimal?)rd["DonGia"] : 0
                    };
                }
                return null;
            }
        }
    }
}