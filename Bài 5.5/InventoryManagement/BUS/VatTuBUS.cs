
using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class VatTuBUS
    {
        VatTuDAL dal = new VatTuDAL();

        public DataTable LayDanhSach()
        {
            return dal.LayDanhSach();
        }

        public DataTable TimKiem(string tuKhoa)
        {
            return dal.TimKiem(tuKhoa);
        }

        public bool Them(VatTuDTO vt)
        {
            if (vt.MaVatTu.Trim() == "" ||
                vt.TenVatTu.Trim() == "")
                return false;

            if (vt.SoLuongTon < 0)
                return false;

            return dal.Them(vt);
        }

        public bool Sua(VatTuDTO vt)
        {
            if (vt.MaVatTu.Trim() == "" ||
                vt.TenVatTu.Trim() == "")
                return false;

            if (vt.SoLuongTon < 0)
                return false;

            return dal.Sua(vt);
        }

        public bool Xoa(string maVatTu)
        {
            if (maVatTu.Trim() == "")
                return false;

            return dal.Xoa(maVatTu);
        }
    }
}
