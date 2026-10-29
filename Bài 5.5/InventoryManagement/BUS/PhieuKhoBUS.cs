
using System.Collections.Generic;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class PhieuKhoBUS
    {
        PhieuKhoDAL dal = new PhieuKhoDAL();

        public int LuuPhieu(
            string loaiPhieu,
            int? maNCC,
            List<ChiTietPhieuKhoDTO> chiTiet)
        {
            return dal.LuuPhieu(loaiPhieu, maNCC, chiTiet);
        }
    }
}
