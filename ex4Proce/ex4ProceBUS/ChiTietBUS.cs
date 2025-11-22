using System.Collections.Generic;
using ex4ProceDAO;
using ex4ProceDTO;

namespace ex4ProceBUS
{
    public class ChiTietBUS
    {
        private ChiTietDAO dao = new ChiTietDAO();

        public List<ChiTietDTO> LayDanhSachChiTiet()
        {
            return dao.LayDanhSachChiTiet();
        }

        public string ThemHoacCapNhat(string maPN, string maSP, int soLuong, decimal donGia, bool cheDoSua)
        {
            if (soLuong <= 0) return "Số lượng phải lớn hơn 0.";
            if (donGia < 0) return "Đơn giá không hợp lệ.";

            ChiTietDTO ct = new ChiTietDTO
            {
                MaPN = maPN,
                MaSP = maSP,
                SoLuong = soLuong,
                DonGia = donGia
            };

            if (cheDoSua)
            {
                if (dao.CapNhat(ct)) return null;
                return "Lỗi khi cập nhật sản phẩm.";
            }

            else
            {
                bool daTonTai = dao.KiemTraTonTai(maPN, maSP);

                if (daTonTai)
                {
                    return "Sản phẩm này đã có trong phiếu. Vui lòng chọn chức năng Sửa để cập nhật số lượng.";

                }
                else
                {
                    if (dao.Them(ct)) return null;
                    return "Lỗi khi thêm sản phẩm mới.";
                }
            }
        }

        public string XoaChiTiet(string maPN, string maSP)
        {
            if (dao.Xoa(maPN, maSP)) return null;
            return "Lỗi khi xóa sản phẩm.";
        }

        public decimal TinhTongTien(string maPN)
        {
            List<ChiTietDTO> allChiTiet = dao.LayDanhSachChiTiet();

            decimal tongTien = 0;

            foreach (ChiTietDTO item in allChiTiet)
            {
                if (item.MaPN == maPN)
                {
                    tongTien += (decimal)(item.SoLuong * item.DonGia);
                }
            }

            return tongTien;
        }
    }
}