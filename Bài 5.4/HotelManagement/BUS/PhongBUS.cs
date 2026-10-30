
using System.Data;
using HotelManagement.DAL;

namespace HotelManagement.BUS
{
    public class PhongBUS
    {
        PhongDAL dal = new PhongDAL();

        public DataTable LayDanhSach()
        {
            return dal.LayDanhSach();
        }

        public void Them(string so, string loai, decimal gia, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(so))
                throw new System.Exception("Chưa nhập số phòng.");

            if (gia < 0)
                throw new System.Exception("Giá phòng không hợp lệ.");

            dal.Them(so, loai, gia, trangThai);
        }

        public void Sua(string so, string loai, decimal gia, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(so))
                throw new System.Exception("Chưa chọn phòng.");

            if (gia < 0)
                throw new System.Exception("Giá phòng không hợp lệ.");

            dal.Sua(so, loai, gia, trangThai);
        }
    }
}
