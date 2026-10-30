
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL
{
    public class KhachHangDAL
    {
        public KhachHangDTO TimTheoCCCD(string cccd)
        {
            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT * FROM KhachHang WHERE CCCD=@cccd", conn))
            {
                cmd.Parameters.Add("@cccd", SqlDbType.VarChar, 20).Value = cccd;
                conn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;

                    return new KhachHangDTO
                    {
                        MaKH = Convert.ToInt32(rd["MaKH"]),
                        HoTen = rd["HoTen"].ToString(),
                        CCCD = rd["CCCD"].ToString(),
                        SoDienThoai = rd["SoDienThoai"] == DBNull.Value
                            ? "" : rd["SoDienThoai"].ToString()
                    };
                }
            }
        }

        public int Them(KhachHangDTO kh)
        {
            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO KhachHang(HoTen, CCCD, SoDienThoai)
                  VALUES(@ten, @cccd, @sdt);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
            {
                cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 100).Value = kh.HoTen;
                cmd.Parameters.Add("@cccd", SqlDbType.VarChar, 20).Value = kh.CCCD;
                cmd.Parameters.Add("@sdt", SqlDbType.VarChar, 15).Value =
                    string.IsNullOrWhiteSpace(kh.SoDienThoai)
                        ? (object)DBNull.Value : kh.SoDienThoai;

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
