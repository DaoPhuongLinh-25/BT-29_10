
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CafePOS
{
    public partial class Form1 : Form
    {
        FlowLayoutPanel pnlBan = new FlowLayoutPanel();
        DataGridView dgvHoaDon = new DataGridView();
        ComboBox cboDanhMuc = new ComboBox();
        ComboBox cboMon = new ComboBox();
        NumericUpDown numSoLuong = new NumericUpDown();
        Label lblBan = new Label();
        Label lblTongTien = new Label();

        int maBan = -1;
        int maHoaDon = -1;

        public Form1()
        {
            InitializeComponent();

            Text = "CAFE POS - QUẢN LÝ QUÁN CAFE";
            Size = new Size(1150, 700);
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();
            TaiDanhMuc();
            TaiBan();
        }

        void TaoGiaoDien()
        {
            Controls.Clear();

            Label title = new Label();
            title.Text = "QUẢN LÝ QUÁN CAFE";
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.Dock = DockStyle.Top;
            title.Height = 55;
            title.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(title);

            pnlBan.Location = new Point(15, 75);
            pnlBan.Size = new Size(350, 520);
            pnlBan.BorderStyle = BorderStyle.FixedSingle;
            pnlBan.Padding = new Padding(5);
            Controls.Add(pnlBan);

            Label lblSoDo = new Label();
            lblSoDo.Text = "SƠ ĐỒ BÀN";
            lblSoDo.Location = new Point(15, 55);
            lblSoDo.AutoSize = true;
            lblSoDo.Font = new Font("Arial", 11, FontStyle.Bold);
            Controls.Add(lblSoDo);
            lblSoDo.BringToFront();

            lblBan.Text = "Chưa chọn bàn";
            lblBan.Location = new Point(390, 80);
            lblBan.AutoSize = true;
            lblBan.Font = new Font("Arial", 12, FontStyle.Bold);
            Controls.Add(lblBan);

            Label lblDM = new Label();
            lblDM.Text = "Danh mục:";
            lblDM.Location = new Point(390, 120);
            lblDM.AutoSize = true;
            Controls.Add(lblDM);

            cboDanhMuc.Location = new Point(475, 115);
            cboDanhMuc.Width = 220;
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.SelectedIndexChanged += (s, e) => TaiMon();
            Controls.Add(cboDanhMuc);

            Label lblMon = new Label();
            lblMon.Text = "Món ăn:";
            lblMon.Location = new Point(390, 160);
            lblMon.AutoSize = true;
            Controls.Add(lblMon);

            cboMon.Location = new Point(475, 155);
            cboMon.Width = 220;
            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboMon);

            Label lblSL = new Label();
            lblSL.Text = "Số lượng:";
            lblSL.Location = new Point(390, 200);
            lblSL.AutoSize = true;
            Controls.Add(lblSL);

            numSoLuong.Location = new Point(475, 195);
            numSoLuong.Minimum = 1;
            numSoLuong.Maximum = 100;
            numSoLuong.Value = 1;
            Controls.Add(numSoLuong);

            Button btnThem = new Button();
            btnThem.Text = "Thêm món";
            btnThem.Location = new Point(475, 235);
            btnThem.Size = new Size(110, 35);
            btnThem.Click += (s, e) => ThemMon();
            Controls.Add(btnThem);

            Button btnThanhToan = new Button();
            btnThanhToan.Text = "Thanh toán";
            btnThanhToan.Location = new Point(590, 235);
            btnThanhToan.Size = new Size(110, 35);
            btnThanhToan.Click += (s, e) => ThanhToan();
            Controls.Add(btnThanhToan);

            dgvHoaDon.Location = new Point(390, 290);
            dgvHoaDon.Size = new Size(710, 250);
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvHoaDon);

            lblTongTien.Text = "Tổng tiền: 0 đ";
            lblTongTien.Location = new Point(390, 555);
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Arial", 14, FontStyle.Bold);
            Controls.Add(lblTongTien);

            Button btnTaiLai = new Button();
            btnTaiLai.Text = "Tải lại sơ đồ bàn";
            btnTaiLai.Location = new Point(15, 605);
            btnTaiLai.Size = new Size(150, 35);
            btnTaiLai.Click += (s, e) => TaiBan();
            Controls.Add(btnTaiLai);
        }

        void TaiDanhMuc()
        {
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    MySqlDataAdapter da = new MySqlDataAdapter(
                        "SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc",
                        conn);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboDanhMuc.DataSource = dt;
                    cboDanhMuc.DisplayMember = "TenDanhMuc";
                    cboDanhMuc.ValueMember = "MaDanhMuc";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }

        void TaiMon()
        {
            if (cboDanhMuc.SelectedValue == null) return;
            if (!(cboDanhMuc.SelectedValue is int)) return;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    MySqlDataAdapter da = new MySqlDataAdapter(
                        "SELECT MaMon, TenMon, DonGia FROM MonAn " +
                        "WHERE MaDanhMuc = @ma",
                        conn);

                    da.SelectCommand.Parameters.AddWithValue(
                        "@ma", cboDanhMuc.SelectedValue);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboMon.DataSource = dt;
                    cboMon.DisplayMember = "TenMon";
                    cboMon.ValueMember = "MaMon";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải món: " + ex.Message);
            }
        }

        void TaiBan()
        {
            try
            {
                pnlBan.Controls.Clear();

                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql =
                        "SELECT MaBan, TenBan, TrangThai FROM BanAn";

                    MySqlDataAdapter da =
                        new MySqlDataAdapter(sql, conn);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["MaBan"]);
                        string ten = row["TenBan"].ToString();
                        string trangThai = row["TrangThai"].ToString();

                        Button btn = new Button();
                        btn.Text = ten + "\n" + trangThai;
                        btn.Size = new Size(100, 75);
                        btn.BackColor = trangThai == "Trống"
                            ? Color.LightGreen : Color.LightCoral;
                        btn.Tag = id;
                        btn.Click += ChonBan_Click;

                        pnlBan.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bàn: " + ex.Message);
            }
        }

        void ChonBan_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            maBan = Convert.ToInt32(btn.Tag);

            lblBan.Text = "Đang chọn: " +
                btn.Text.Replace("\n", " - ");

            maHoaDon = -1;
            dgvHoaDon.DataSource = null;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT MaHoaDon FROM HoaDon " +
                        "WHERE MaBan=@ban AND TrangThai=0 " +
                        "ORDER BY MaHoaDon DESC LIMIT 1", conn);

                    cmd.Parameters.AddWithValue("@ban", maBan);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        maHoaDon = Convert.ToInt32(result);
                        TaiChiTietHoaDon();
                    }
                    else
                    {
                        lblTongTien.Text = "Tổng tiền: 0 đ";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chọn bàn: " + ex.Message);
            }
        }

        void TaiChiTietHoaDon()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string sql =
                    "SELECT m.TenMon, c.SoLuong, c.DonGia, " +
                    "(c.SoLuong*c.DonGia) AS ThanhTien " +
                    "FROM ChiTietHoaDon c " +
                    "JOIN MonAn m ON c.MaMon=m.MaMon " +
                    "WHERE c.MaHoaDon=@hd";

                MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@hd", maHoaDon);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvHoaDon.DataSource = dt;

                decimal tong = 0;
                foreach (DataRow row in dt.Rows)
                    tong += Convert.ToDecimal(row["ThanhTien"]);

                lblTongTien.Text = "Tổng tiền: " +
                    tong.ToString("N0") + " đ";
            }
        }

        void ThemMon()
        {
            if (maBan == -1)
            {
                MessageBox.Show("Bạn hãy chọn bàn trước!");
                return;
            }

            if (cboMon.SelectedValue == null ||
                !(cboMon.SelectedValue is int))
            {
                MessageBox.Show("Bạn hãy chọn món ăn!");
                return;
            }

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    MySqlTransaction tran = conn.BeginTransaction();

                    try
                    {
                        if (maHoaDon == -1)
                        {
                            MySqlCommand cmdHD = new MySqlCommand(
                                "INSERT INTO HoaDon " +
                                "(MaBan, GioVao, TongTien, TrangThai) " +
                                "VALUES (@ban, NOW(), 0, 0); " +
                                "SELECT LAST_INSERT_ID();",
                                conn, tran);

                            cmdHD.Parameters.AddWithValue("@ban", maBan);
                            maHoaDon = Convert.ToInt32(
                                cmdHD.ExecuteScalar());

                            MySqlCommand cmdBan = new MySqlCommand(
                                "UPDATE BanAn SET TrangThai='Có khách' " +
                                "WHERE MaBan=@ban", conn, tran);

                            cmdBan.Parameters.AddWithValue("@ban", maBan);
                            cmdBan.ExecuteNonQuery();
                        }

                        int maMon = Convert.ToInt32(cboMon.SelectedValue);
                        int sl = Convert.ToInt32(numSoLuong.Value);

                        MySqlCommand cmd = new MySqlCommand(
                            "SELECT DonGia FROM MonAn WHERE MaMon=@mon",
                            conn, tran);

                        cmd.Parameters.AddWithValue("@mon", maMon);
                        decimal gia = Convert.ToDecimal(cmd.ExecuteScalar());

                        MySqlCommand cmdCT = new MySqlCommand(
                            "SELECT SoLuong FROM ChiTietHoaDon " +
                            "WHERE MaHoaDon=@hd AND MaMon=@mon",
                            conn, tran);

                        cmdCT.Parameters.AddWithValue("@hd", maHoaDon);
                        cmdCT.Parameters.AddWithValue("@mon", maMon);

                        object old = cmdCT.ExecuteScalar();

                        if (old != null)
                        {
                            MySqlCommand update = new MySqlCommand(
                                "UPDATE ChiTietHoaDon " +
                                "SET SoLuong=SoLuong+@sl " +
                                "WHERE MaHoaDon=@hd AND MaMon=@mon",
                                conn, tran);

                            update.Parameters.AddWithValue("@sl", sl);
                            update.Parameters.AddWithValue("@hd", maHoaDon);
                            update.Parameters.AddWithValue("@mon", maMon);
                            update.ExecuteNonQuery();
                        }
                        else
                        {
                            MySqlCommand insert = new MySqlCommand(
                                "INSERT INTO ChiTietHoaDon " +
                                "(MaHoaDon, MaMon, SoLuong, DonGia) " +
                                "VALUES (@hd, @mon, @sl, @gia)",
                                conn, tran);

                            insert.Parameters.AddWithValue("@hd", maHoaDon);
                            insert.Parameters.AddWithValue("@mon", maMon);
                            insert.Parameters.AddWithValue("@sl", sl);
                            insert.Parameters.AddWithValue("@gia", gia);
                            insert.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }

                TaiChiTietHoaDon();
                TaiBan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm món: " + ex.Message);
            }
        }

        void ThanhToan()
        {
            if (maBan == -1 || maHoaDon == -1)
            {
                MessageBox.Show("Bàn chưa có hóa đơn!");
                return;
            }

            if (MessageBox.Show("Xác nhận thanh toán?",
                "Thanh toán", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    MySqlTransaction tran = conn.BeginTransaction();

                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(
                            "SELECT SUM(SoLuong*DonGia) " +
                            "FROM ChiTietHoaDon WHERE MaHoaDon=@hd",
                            conn, tran);

                        cmd.Parameters.AddWithValue("@hd", maHoaDon);

                        decimal tong = Convert.ToDecimal(cmd.ExecuteScalar());

                        MySqlCommand hd = new MySqlCommand(
                            "UPDATE HoaDon SET GioRa=NOW(), " +
                            "TongTien=@tong, TrangThai=1 " +
                            "WHERE MaHoaDon=@hd",
                            conn, tran);

                        hd.Parameters.AddWithValue("@tong", tong);
                        hd.Parameters.AddWithValue("@hd", maHoaDon);
                        hd.ExecuteNonQuery();

                        MySqlCommand ban = new MySqlCommand(
                            "UPDATE BanAn SET TrangThai='Trống' " +
                            "WHERE MaBan=@ban",
                            conn, tran);

                        ban.Parameters.AddWithValue("@ban", maBan);
                        ban.ExecuteNonQuery();

                        tran.Commit();

                        MessageBox.Show("Thanh toán thành công!\nTổng tiền: " +
                            tong.ToString("N0") + " đ");
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }

                maHoaDon = -1;
                TaiBan();
                dgvHoaDon.DataSource = null;
                lblTongTien.Text = "Tổng tiền: 0 đ";
                lblBan.Text = "Chưa chọn bàn";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message);
            }
        }
    }
}
