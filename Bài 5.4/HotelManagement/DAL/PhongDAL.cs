
using System.Data;
using Microsoft.Data.SqlClient;

namespace HotelManagement.DAL
{
    public class PhongDAL
    {
        public DataTable LayDanhSach()
        {
            using (SqlConnection conn = Database.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM Phong", conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void Them(string soPhong, string loaiPhong,
            decimal gia, string trangThai)
        {
            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Phong(SoPhong, LoaiPhong, GiaTheoGio, TrangThai)
                  VALUES(@so, @loai, @gia, @tt)", conn))
            {
                cmd.Parameters.Add("@so", SqlDbType.VarChar, 10).Value = soPhong;
                cmd.Parameters.Add("@loai", SqlDbType.NVarChar, 50).Value = loaiPhong;
                cmd.Parameters.Add("@gia", SqlDbType.Decimal).Value = gia;
                cmd.Parameters["@gia"].Precision = 18;
                cmd.Parameters["@gia"].Scale = 2;
                cmd.Parameters.Add("@tt", SqlDbType.NVarChar, 30).Value = trangThai;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Sua(string soPhong, string loaiPhong,
            decimal gia, string trangThai)
        {
            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Phong
                  SET LoaiPhong=@loai, GiaTheoGio=@gia, TrangThai=@tt
                  WHERE SoPhong=@so", conn))
            {
                cmd.Parameters.Add("@so", SqlDbType.VarChar, 10).Value = soPhong;
                cmd.Parameters.Add("@loai", SqlDbType.NVarChar, 50).Value = loaiPhong;
                cmd.Parameters.Add("@gia", SqlDbType.Decimal).Value = gia;
                cmd.Parameters["@gia"].Precision = 18;
                cmd.Parameters["@gia"].Scale = 2;
                cmd.Parameters.Add("@tt", SqlDbType.NVarChar, 30).Value = trangThai;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
