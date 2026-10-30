
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.BUS;
using HotelManagement.DTO;

namespace HotelManagement.GUI
{
    public class FormCheckIn : Form
    {
        TextBox txtCCCD = new TextBox();
        TextBox txtTen = new TextBox();
        TextBox txtSDT = new TextBox();
        ComboBox cboPhong = new ComboBox();

        KhachHangBUS khBUS = new KhachHangBUS();
        PhongBUS phongBUS = new PhongBUS();
        DatPhongBUS datBUS = new DatPhongBUS();

        public FormCheckIn()
        {
            Text = "Check-in - Nhận phòng";
            Size = new Size(480, 350);
            StartPosition = FormStartPosition.CenterScreen;

            ThemLabel("CCCD:", 25, 25);
            txtCCCD.SetBounds(150, 25, 250, 28);
            Controls.Add(txtCCCD);

            Button btnTim = new Button();
            btnTim.Text = "Tra cứu";
            btnTim.SetBounds(150, 60, 100, 30);
            btnTim.Click += (s, e) => TimKhach();
            Controls.Add(btnTim);

            ThemLabel("Họ tên:", 25, 105);
            txtTen.SetBounds(150, 105, 250, 28);
            Controls.Add(txtTen);

            ThemLabel("Số điện thoại:", 25, 145);
            txtSDT.SetBounds(150, 145, 250, 28);
            Controls.Add(txtSDT);

            ThemLabel("Phòng trống:", 25, 185);
            cboPhong.SetBounds(150, 185, 250, 28);
            cboPhong.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboPhong);

            Button btnNhan = new Button();
            btnNhan.Text = "Nhận phòng";
            btnNhan.SetBounds(150, 235, 130, 35);
            btnNhan.Click += (s, e) => NhanPhong();
            Controls.Add(btnNhan);

            TaiPhongTrong();
        }

        void ThemLabel(string text, int x, int y)
        {
            Controls.Add(new Label
            {
                Text = text,
                Left = x,
                Top = y,
                Width = 120,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft
            });
        }

        void TimKhach()
        {
            try
            {
                KhachHangDTO kh = khBUS.TimTheoCCCD(txtCCCD.Text.Trim());

                if (kh == null)
                {
                    txtTen.Clear();
                    txtSDT.Clear();
                    MessageBox.Show("Khách hàng mới. Hãy nhập họ tên và số điện thoại.");
                }
                else
                {
                    txtTen.Text = kh.HoTen;
                    txtSDT.Text = kh.SoDienThoai;
                    MessageBox.Show("Đã tìm thấy khách hàng.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TaiPhongTrong()
        {
            try
            {
                DataTable dt = phongBUS.LayDanhSach();
                DataView view = new DataView(dt);
                view.RowFilter = "TrangThai = 'Trống'";

                cboPhong.DataSource = view;
                cboPhong.DisplayMember = "SoPhong";
                cboPhong.ValueMember = "SoPhong";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phòng: " + ex.Message);
            }
        }

        void NhanPhong()
        {
            try
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    CCCD = txtCCCD.Text.Trim(),
                    HoTen = txtTen.Text.Trim(),
                    SoDienThoai = txtSDT.Text.Trim()
                };

                if (cboPhong.SelectedValue == null)
                    throw new Exception("Không có phòng trống.");

                int maPhieu = datBUS.CheckIn(kh, cboPhong.SelectedValue.ToString());

                MessageBox.Show("Nhận phòng thành công. Mã phiếu: " + maPhieu);
                TaiPhongTrong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi check-in: " + ex.Message);
            }
        }
    }
}
