
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class PhieuKhoDAL
    {
        public int LuuPhieu(
            string loaiPhieu,
            int? maNCC,
            List<ChiTietPhieuKhoDTO> chiTiet)
        {
            if (loaiPhieu != "NHAP" && loaiPhieu != "XUAT")
                throw new Exception("Loại phiếu không hợp lệ.");

            if (chiTiet == null || chiTiet.Count == 0)
                throw new Exception("Phiếu phải có ít nhất một vật tư.");

            foreach (ChiTietPhieuKhoDTO ct in chiTiet)
            {
                if (string.IsNullOrWhiteSpace(ct.MaVatTu) ||
                    ct.SoLuong <= 0 || ct.DonGia < 0)
                {
                    throw new Exception("Thông tin vật tư không hợp lệ.");
                }
            }

            // Không cho một vật tư xuất hiện nhiều lần trong cùng phiếu
            for (int i = 0; i < chiTiet.Count; i++)
            {
                for (int j = i + 1; j < chiTiet.Count; j++)
                {
                    if (chiTiet[i].MaVatTu == chiTiet[j].MaVatTu)
                        throw new Exception("Vật tư bị trùng trong phiếu.");
                }
            }

            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        decimal tongTien = 0;

                        foreach (ChiTietPhieuKhoDTO ct in chiTiet)
                        {
                            tongTien += ct.SoLuong * ct.DonGia;
                        }

                        string sqlPhieu = @"
                            INSERT INTO PhieuKho
                                (LoaiPhieu, MaNCC, TongTien)
                            VALUES
                                (@loai, @ncc, @tong)";

                        int maPhieu;

                        using (MySqlCommand cmd = new MySqlCommand(
                            sqlPhieu, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@loai", loaiPhieu);
                            cmd.Parameters.AddWithValue("@ncc",
                                maNCC.HasValue
                                    ? (object)maNCC.Value
                                    : DBNull.Value);
                            cmd.Parameters.AddWithValue("@tong", tongTien);

                            cmd.ExecuteNonQuery();
                            maPhieu = Convert.ToInt32(cmd.LastInsertedId);
                        }

                        foreach (ChiTietPhieuKhoDTO ct in chiTiet)
                        {
                            // Kiểm tra vật tư tồn tại và khóa dòng kho
                            string sqlKiemTra =
                                "SELECT SoLuongTon FROM VatTu " +
                                "WHERE MaVatTu=@ma FOR UPDATE";

                            using (MySqlCommand cmd = new MySqlCommand(
                                sqlKiemTra, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@ma", ct.MaVatTu);

                                object ton = cmd.ExecuteScalar();

                                if (ton == null)
                                    throw new Exception(
                                        "Không tìm thấy vật tư: " + ct.MaVatTu);

                                decimal soLuongTon = Convert.ToDecimal(ton);

                                if (loaiPhieu == "XUAT" &&
                                    soLuongTon < ct.SoLuong)
                                {
                                    throw new Exception(
                                        "Không đủ hàng trong kho: " +
                                        ct.MaVatTu);
                                }
                            }

                            string sqlChiTiet = @"
                                INSERT INTO ChiTietPhieuKho
                                    (MaPhieu, MaVatTu, SoLuong, DonGia)
                                VALUES
                                    (@phieu, @ma, @sl, @gia)";

                            using (MySqlCommand cmd = new MySqlCommand(
                                sqlChiTiet, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@phieu", maPhieu);
                                cmd.Parameters.AddWithValue("@ma", ct.MaVatTu);
                                cmd.Parameters.AddWithValue("@sl", ct.SoLuong);
                                cmd.Parameters.AddWithValue("@gia", ct.DonGia);
                                cmd.ExecuteNonQuery();
                            }

                            string sqlCapNhat;

                            if (loaiPhieu == "NHAP")
                            {
                                sqlCapNhat = @"
                                    UPDATE VatTu
                                    SET SoLuongTon = SoLuongTon + @sl
                                    WHERE MaVatTu = @ma";
                            }
                            else
                            {
                                sqlCapNhat = @"
                                    UPDATE VatTu
                                    SET SoLuongTon = SoLuongTon - @sl
                                    WHERE MaVatTu = @ma
                                      AND SoLuongTon >= @sl";
                            }

                            using (MySqlCommand cmd = new MySqlCommand(
                                sqlCapNhat, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@sl", ct.SoLuong);
                                cmd.Parameters.AddWithValue("@ma", ct.MaVatTu);

                                int soDong = cmd.ExecuteNonQuery();

                                if (soDong == 0)
                                    throw new Exception(
                                        "Không đủ hàng trong kho: " +
                                        ct.MaVatTu);
                            }
                        }

                        trans.Commit();
                        return maPhieu;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
