
using System.Data;
using MySql.Data.MySqlClient;

namespace InventoryManagement.DAL
{
    public class BaoCaoDAL
    {
        public DataTable CanhBaoTonKho(decimal nguong)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT MaVatTu, TenVatTu, DVT, SoLuongTon
                    FROM VatTu
                    WHERE SoLuongTon < @nguong";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nguong", nguong);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public decimal TongGiaTriTonKho()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT COALESCE(
                        SUM(v.SoLuongTon * COALESCE(g.DonGiaNhap, 0)), 0)
                    FROM VatTu v
                    LEFT JOIN (
                        SELECT MaVatTu, DonGia AS DonGiaNhap
                        FROM ChiTietPhieuKho ct
                        JOIN PhieuKho p ON p.MaPhieu = ct.MaPhieu
                        WHERE p.LoaiPhieu = 'NHAP'
                          AND ct.MaPhieu = (
                              SELECT MAX(ct2.MaPhieu)
                              FROM ChiTietPhieuKho ct2
                              JOIN PhieuKho p2
                                  ON p2.MaPhieu = ct2.MaPhieu
                              WHERE ct2.MaVatTu = ct.MaVatTu
                                AND p2.LoaiPhieu = 'NHAP'
                          )
                    ) g ON v.MaVatTu = g.MaVatTu";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    object kq = cmd.ExecuteScalar();
                    return System.Convert.ToDecimal(kq);
                }
            }
        }
    }
}
