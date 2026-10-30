
using System.Data;
using HotelManagement.DAL;

namespace HotelManagement.BUS
{
    public class DichVuBUS
    {
        DichVuDAL dal = new DichVuDAL();

        public DataTable LayDanhSach()
        {
            return dal.LayDanhSach();
        }
    }
}
