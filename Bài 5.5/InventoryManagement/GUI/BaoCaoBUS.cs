
using System.Data;
using InventoryManagement.DAL;

namespace InventoryManagement.BUS
{
    public class BaoCaoBUS
    {
        BaoCaoDAL dal = new BaoCaoDAL();

        public DataTable CanhBaoTonKho(decimal nguong)
        {
            return dal.CanhBaoTonKho(nguong);
        }

        public decimal TongGiaTriTonKho()
        {
            return dal.TongGiaTriTonKho();
        }
    }
}
