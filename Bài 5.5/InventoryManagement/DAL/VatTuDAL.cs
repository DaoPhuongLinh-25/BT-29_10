
using System;
using System.Data;
using MySql.Data.MySqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class VatTuDAL
    {
        public DataTable LayDanhSach()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                string sql = "SELECT * FROM VatTu";
                MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable TimKiem(string tuKhoa)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                string sql = @"SELECT * FROM VatTu
                               WHERE MaVatTu LIKE @tuKhoa
                                  OR TenVatTu LIKE @tuKhoa";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool Them(VatTuDTO vt)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO VatTu
                    (MaVatTu, TenVatTu, DVT, SoLuongTon)
                    VALUES (@ma, @ten, @dvt, @ton)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", vt.MaVatTu);
                    cmd.Parameters.AddWithValue("@ten", vt.TenVatTu);
                    cmd.Parameters.AddWithValue("@dvt", vt.DVT);
                    cmd.Parameters.AddWithValue("@ton", vt.SoLuongTon);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Sua(VatTuDTO vt)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE VatTu
                    SET TenVatTu=@ten, DVT=@dvt, SoLuongTon=@ton
                    WHERE MaVatTu=@ma";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", vt.MaVatTu);
                    cmd.Parameters.AddWithValue("@ten", vt.TenVatTu);
                    cmd.Parameters.AddWithValue("@dvt", vt.DVT);
                    cmd.Parameters.AddWithValue("@ton", vt.SoLuongTon);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Xoa(string maVatTu)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = "DELETE FROM VatTu WHERE MaVatTu=@ma";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maVatTu);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
