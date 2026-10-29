
using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class NhaCungCapBUS
    {
        NhaCungCapDAL dal = new NhaCungCapDAL();

        public DataTable LayDanhSach()
        {
            return dal.LayDanhSach();
        }

        public DataTable TimKiem(string tuKhoa)
        {
            return dal.TimKiem(tuKhoa);
        }

        public bool Them(NhaCungCapDTO ncc)
        {
            if (ncc.TenNCC.Trim() == "")
                return false;

            return dal.Them(ncc);
        }

        public bool Sua(NhaCungCapDTO ncc)
        {
            if (ncc.MaNCC <= 0 || ncc.TenNCC.Trim() == "")
                return false;

            return dal.Sua(ncc);
        }

        public bool Xoa(int maNCC)
        {
            if (maNCC <= 0)
                return false;

            return dal.Xoa(maNCC);
        }
    }
}
