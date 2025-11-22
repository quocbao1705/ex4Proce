using System.Collections.Generic;
using ex4ProceDAO;
using ex4ProceDTO;

namespace ex4ProceBUS
{
    public class NhaCungCapBUS
    {
        private NhaCungCapDAO dao = new NhaCungCapDAO();

        public List<NhaCungCapDTO> LayDanhSach()
        {
            return dao.LayDanhSach();
        }
    }
}