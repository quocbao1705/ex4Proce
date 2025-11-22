using System;
using System.Collections.Generic;
using ex4ProceDAO;
using ex4ProceDTO;

namespace ex4ProceBUS
{
    public class PhieuNhapBUS
    {
        private PhieuNhapDAO dao = new PhieuNhapDAO();

        public List<PhieuNhapDTO> LayDanhSach()
        {
            return dao.LayDanhSach();
        }

        public string ThemPhieuNhap(string maPN, string maNCC, DateTime ngayNhap)
        {
            if (string.IsNullOrWhiteSpace(maPN))
                return "Mã phiếu nhập không được để trống.";
            if (string.IsNullOrWhiteSpace(maNCC))
                return "Vui lòng chọn nhà cung cấp.";

            PhieuNhapDTO pn = new PhieuNhapDTO();
            pn.MaPN = maPN;
            pn.MaNCC = maNCC;
            pn.NgayNhap = ngayNhap;

            if (dao.Them(pn))
            {
                return null;
            }
            else
            {
                return "Lỗi khi thêm phiếu nhập. Có thể Mã Phiếu đã tồn tại.";
            }
        }
    }
}