
using System;
using System.Data;
using System.Windows.Forms;
using HotelManagement.BUS;

namespace HotelManagement.GUI
{
    public class FormQuanLyPhong : Form
    {
        DataGridView dgv = new DataGridView();
        ComboBox cboLoai = new ComboBox();
        ComboBox cboTrangThai = new ComboBox();
        DataTable dt = new DataTable();
        PhongBUS bus = new PhongBUS();

        public FormQuanLyPhong()
        {
            Text = "Quản lý phòng";
            Width = 850;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;

            cboLoai.SetBounds(20, 20, 180, 30);
            cboLoai.Items.AddRange(new object[] { "Tất cả", "Đơn", "Đôi", "VIP" });
            cboLoai.SelectedIndex = 0;
            cboLoai.SelectedIndexChanged += (s, e) => LocPhong();
            Controls.Add(cboLoai);

            cboTrangThai.SetBounds(220, 20, 200, 30);
            cboTrangThai.Items.AddRange(new object[]
                { "Tất cả", "Trống", "Đang ở", "Đang dọn dẹp" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += (s, e) => LocPhong();
            Controls.Add(cboTrangThai);

            Button btnTai = new Button();
            btnTai.Text = "Tải lại";
            btnTai.SetBounds(440, 18, 100, 32);
            btnTai.Click += (s, e) => TaiDuLieu();
            Controls.Add(btnTai);

            dgv.SetBounds(20, 70, 790, 350);
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            Controls.Add(dgv);

            TaiDuLieu();
        }

        void TaiDuLieu()
        {
            try
            {
                dt = bus.LayDanhSach();
                dgv.DataSource = dt;
                LocPhong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phòng: " + ex.Message);
            }
        }

        void LocPhong()
        {
            if (dt == null || dt.Columns.Count == 0) return;

            DataView view = new DataView(dt);
            string dieuKien = "";

            if (cboLoai.Text != "Tất cả" && cboLoai.Text != "")
                dieuKien = "LoaiPhong = '" + cboLoai.Text.Replace("'", "''") + "'";

            if (cboTrangThai.Text != "Tất cả" && cboTrangThai.Text != "")
            {
                if (dieuKien != "") dieuKien += " AND ";
                dieuKien += "TrangThai = '" +
                    cboTrangThai.Text.Replace("'", "''") + "'";
            }

            view.RowFilter = dieuKien;
            dgv.DataSource = view;
        }
    }
}
