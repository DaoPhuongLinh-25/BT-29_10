
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public class KhachHangBUS
    {
        KhachHangDAL dal = new KhachHangDAL();

        public KhachHangDTO TimTheoCCCD(string cccd)
        {
            if (string.IsNullOrWhiteSpace(cccd))
                throw new System.Exception("Vui lòng nhập CCCD.");

            return dal.TimTheoCCCD(cccd);
        }

        public int Them(KhachHangDTO kh)
        {
            if (string.IsNullOrWhiteSpace(kh.HoTen))
                throw new System.Exception("Vui lòng nhập họ tên.");

            if (string.IsNullOrWhiteSpace(kh.CCCD))
                throw new System.Exception("Vui lòng nhập CCCD.");

            return dal.Them(kh);
        }
    }
}
