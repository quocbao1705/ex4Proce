using System.Collections.Generic;
using ex4ProceDAO;
using ex4ProceDTO;

namespace ex4ProceBUS
{
    public class SanPhamBUS
    {
        private SanPhamDAO dao = new SanPhamDAO();

        public List<SanPhamDTO> LayDanhSach()
        {
            return dao.LayDanhSach();
        }

        public SanPhamDTO LayTheoMa(string maSP)
        {
            return dao.LayTheoMa(maSP);
        }
    }
}