using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ex4ProceDTO;

namespace ex4ProceDAO
{
    public class ChiTietDAO
    {
        DBConnection db = new DBConnection();

        public List<ChiTietDTO> LayDanhSachChiTiet()
        {
            List<ChiTietDTO> list = new List<ChiTietDTO>();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetAllChiTiet", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new ChiTietDTO
                    {
                        MaPN = rd["MaPN"].ToString(),
                        MaSP = rd["MaSP"].ToString(),
                        SoLuong = int.Parse(rd["SoLuong"].ToString()),
                        DonGia = decimal.Parse(rd["DonGia"].ToString()),
                    });
                }
            }
            return list;
        }

        public bool KiemTraTonTai(string maPN, string maSP)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_CheckChiTietExist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaPN", maPN);
                cmd.Parameters.AddWithValue("@MaSP", maSP);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool Them(ChiTietDTO ct)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertChiTiet", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaPN", ct.MaPN);
                    cmd.Parameters.AddWithValue("@MaSP", ct.MaSP);
                    cmd.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
                    cmd.Parameters.AddWithValue("@DonGia", ct.DonGia);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception) { return false; }
            }
        }

        public bool CapNhat(ChiTietDTO ct)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_UpdateChiTiet", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaPN", ct.MaPN);
                    cmd.Parameters.AddWithValue("@MaSP", ct.MaSP);
                    cmd.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
                    cmd.Parameters.AddWithValue("@DonGia", ct.DonGia);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception) { return false; }
            }
        }

        public bool Xoa(string maPN, string maSP)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteChiTiet", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaPN", maPN);
                    cmd.Parameters.AddWithValue("@MaSP", maSP);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception) { return false; }
            }
        }
    }
}