
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class DatPhongDAL
    {
        public int CheckIn(KhachHangDTO kh, string soPhong)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int maKH;

                        using (SqlCommand cmd = new SqlCommand(
                            "SELECT MaKH FROM KhachHang WHERE CCCD=@cccd",
                            conn, trans))
                        {
                            cmd.Parameters.Add("@cccd", SqlDbType.VarChar, 20)
                                .Value = kh.CCCD;
                            object kq = cmd.ExecuteScalar();

                            if (kq == null)
                            {
                                using (SqlCommand them = new SqlCommand(
                                    @"INSERT INTO KhachHang(HoTen,CCCD,SoDienThoai)
                                      VALUES(@ten,@cccd,@sdt);
                                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
                                    conn, trans))
                                {
                                    them.Parameters.Add("@ten", SqlDbType.NVarChar, 100)
                                        .Value = kh.HoTen;
                                    them.Parameters.Add("@cccd", SqlDbType.VarChar, 20)
                                        .Value = kh.CCCD;
                                    them.Parameters.Add("@sdt", SqlDbType.VarChar, 15)
                                        .Value = string.IsNullOrWhiteSpace(kh.SoDienThoai)
                                            ? (object)DBNull.Value : kh.SoDienThoai;
                                    maKH = Convert.ToInt32(them.ExecuteScalar());
                                }
                            }
                            else
                            {
                                maKH = Convert.ToInt32(kq);
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(
                            @"UPDATE Phong SET TrangThai=N'Đang ở'
                              WHERE SoPhong=@so AND TrangThai=N'Trống'",
                            conn, trans))
                        {
                            cmd.Parameters.Add("@so", SqlDbType.VarChar, 10).Value = soPhong;

                            if (cmd.ExecuteNonQuery() != 1)
                                throw new Exception("Phòng không tồn tại hoặc không còn trống.");
                        }

                        using (SqlCommand cmd = new SqlCommand(
                            @"INSERT INTO PhieuDatPhong(SoPhong,MaKH,NgayNhanPhong)
                              VALUES(@so,@kh,GETDATE());
                              SELECT CAST(SCOPE_IDENTITY() AS INT);",
                            conn, trans))
                        {
                            cmd.Parameters.Add("@so", SqlDbType.VarChar, 10).Value = soPhong;
                            cmd.Parameters.Add("@kh", SqlDbType.Int).Value = maKH;
                            int maPhieu = Convert.ToInt32(cmd.ExecuteScalar());
                            trans.Commit();
                            return maPhieu;
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void ThemDichVu(int maPhieu, int maDV, int soLuong)
        {
            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO SuDungDichVu(MaPhieu,MaDV,SoLuong,ThoiGian)
                  SELECT @phieu,@dv,@sl,GETDATE()
                  WHERE EXISTS (
                      SELECT 1 FROM PhieuDatPhong
                      WHERE MaPhieu=@phieu AND NgayTraPhong IS NULL
                  )", conn))
            {
                cmd.Parameters.Add("@phieu", SqlDbType.Int).Value = maPhieu;
                cmd.Parameters.Add("@dv", SqlDbType.Int).Value = maDV;
                cmd.Parameters.Add("@sl", SqlDbType.Int).Value = soLuong;

                conn.Open();
                if (cmd.ExecuteNonQuery() == 0)
                    throw new Exception("Phiếu không tồn tại hoặc đã check-out.");
            }
        }

        public decimal CheckOut(int maPhieu)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string soPhong;
                        DateTime ngayNhan;

                        using (SqlCommand cmd = new SqlCommand(
                            @"SELECT SoPhong, NgayNhanPhong
                              FROM PhieuDatPhong WITH (UPDLOCK, HOLDLOCK)
                              WHERE MaPhieu=@ma AND NgayTraPhong IS NULL",
                            conn, trans))
                        {
                            cmd.Parameters.Add("@ma", SqlDbType.Int).Value = maPhieu;

                            using (SqlDataReader rd = cmd.ExecuteReader())
                            {
                                if (!rd.Read())
                                    throw new Exception("Không tìm thấy phiếu đang lưu trú.");

                                soPhong = rd["SoPhong"].ToString();
                                ngayNhan = Convert.ToDateTime(rd["NgayNhanPhong"]);
                            }
                        }

                        decimal giaGio;

                        using (SqlCommand cmd = new SqlCommand(
                            "SELECT GiaTheoGio FROM Phong WHERE SoPhong=@so",
                            conn, trans))
                        {
                            cmd.Parameters.Add("@so", SqlDbType.VarChar, 10).Value = soPhong;
                            giaGio = Convert.ToDecimal(cmd.ExecuteScalar());
                        }

                        decimal tongDichVu;

                        using (SqlCommand cmd = new SqlCommand(
                            @"SELECT ISNULL(SUM(sd.SoLuong * dv.DonGia),0)
                              FROM SuDungDichVu sd
                              JOIN DichVu dv ON sd.MaDV=dv.MaDV
                              WHERE sd.MaPhieu=@ma", conn, trans))
                        {
                            cmd.Parameters.Add("@ma", SqlDbType.Int).Value = maPhieu;
                            tongDichVu = Convert.ToDecimal(cmd.ExecuteScalar());
                        }

                        DateTime ngayTra = DateTime.Now;
                        decimal soGio = (decimal)(ngayTra - ngayNhan).TotalHours;
                        if (soGio < 1) soGio = 1;

                        decimal tongTien = Math.Ceiling(soGio) * giaGio + tongDichVu;

                        using (SqlCommand cmd = new SqlCommand(
                            @"UPDATE PhieuDatPhong
                              SET NgayTraPhong=@ngay, TongTienThanhToan=@tong
                              WHERE MaPhieu=@ma AND NgayTraPhong IS NULL",
                            conn, trans))
                        {
                            cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = ngayTra;
                            cmd.Parameters.Add("@tong", SqlDbType.Decimal).Value = tongTien;
                            cmd.Parameters["@tong"].Precision = 18;
                            cmd.Parameters["@tong"].Scale = 2;
                            cmd.Parameters.Add("@ma", SqlDbType.Int).Value = maPhieu;

                            if (cmd.ExecuteNonQuery() != 1)
                                throw new Exception("Không thể cập nhật phiếu thanh toán.");
                        }

                        using (SqlCommand cmd = new SqlCommand(
                            "UPDATE Phong SET TrangThai=N'Đang dọn dẹp' WHERE SoPhong=@so",
                            conn, trans))
                        {
                            cmd.Parameters.Add("@so", SqlDbType.VarChar, 10).Value = soPhong;
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return tongTien;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public DataTable LayPhieuDangO()
        {
            using (SqlConnection conn = Database.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT p.MaPhieu, p.SoPhong, k.HoTen, k.CCCD, p.NgayNhanPhong
                  FROM PhieuDatPhong p
                  JOIN KhachHang k ON p.MaKH=k.MaKH
                  WHERE p.NgayTraPhong IS NULL", conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
