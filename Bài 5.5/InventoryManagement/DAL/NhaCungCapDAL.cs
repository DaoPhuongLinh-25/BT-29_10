
using System.Data;
using MySql.Data.MySqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class NhaCungCapDAL
    {
        public DataTable LayDanhSach()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter(
                    "SELECT * FROM NhaCungCap", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable TimKiem(string tuKhoa)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                string sql = @"SELECT * FROM NhaCungCap
                    WHERE TenNCC LIKE @tk
                       OR DienThoai LIKE @tk";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tk", "%" + tuKhoa + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public bool Them(NhaCungCapDTO ncc)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO NhaCungCap
                    (TenNCC, DienThoai, DiaChi)
                    VALUES (@ten, @sdt, @dc)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", ncc.TenNCC);
                    cmd.Parameters.AddWithValue("@sdt",
                        string.IsNullOrWhiteSpace(ncc.DienThoai)
                            ? DBNull.Value : ncc.DienThoai);
                    cmd.Parameters.AddWithValue("@dc", ncc.DiaChi);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Sua(NhaCungCapDTO ncc)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE NhaCungCap
                    SET TenNCC=@ten, DienThoai=@sdt, DiaChi=@dc
                    WHERE MaNCC=@ma";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", ncc.MaNCC);
                    cmd.Parameters.AddWithValue("@ten", ncc.TenNCC);
                    cmd.Parameters.AddWithValue("@sdt",
                        string.IsNullOrWhiteSpace(ncc.DienThoai)
                            ? DBNull.Value : ncc.DienThoai);
                    cmd.Parameters.AddWithValue("@dc", ncc.DiaChi);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Xoa(int maNCC)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql = "DELETE FROM NhaCungCap WHERE MaNCC=@ma";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma", maNCC);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
