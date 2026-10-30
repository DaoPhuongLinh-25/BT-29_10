
using System;
using System.Data;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public class DatPhongBUS
    {
        DatPhongDAL dal = new DatPhongDAL();

        public int CheckIn(KhachHangDTO kh, string soPhong)
        {
            if (kh == null)
                throw new Exception("Thông tin khách hàng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(kh.HoTen) ||
                string.IsNullOrWhiteSpace(kh.CCCD))
                throw new Exception("Vui lòng nhập họ tên và CCCD.");

            if (string.IsNullOrWhiteSpace(soPhong))
                throw new Exception("Vui lòng chọn phòng.");

            return dal.CheckIn(kh, soPhong);
        }

        public void ThemDichVu(int maPhieu, int maDV, int soLuong)
        {
            if (maPhieu <= 0 || maDV <= 0 || soLuong <= 0)
                throw new Exception("Thông tin dịch vụ không hợp lệ.");

            dal.ThemDichVu(maPhieu, maDV, soLuong);
        }

        public decimal CheckOut(int maPhieu)
        {
            if (maPhieu <= 0)
                throw new Exception("Vui lòng chọn phiếu đang lưu trú.");

            return dal.CheckOut(maPhieu);
        }

        public DataTable LayPhieuDangO()
        {
            return dal.LayPhieuDangO();
        }
    }
}
