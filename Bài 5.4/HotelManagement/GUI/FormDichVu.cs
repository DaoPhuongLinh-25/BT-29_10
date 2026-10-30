
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.BUS;

namespace HotelManagement.GUI
{
    public class FormDichVu : Form
    {
        ComboBox cboPhieu = new ComboBox();
        ComboBox cboDichVu = new ComboBox();
        NumericUpDown numSoLuong = new NumericUpDown();

        DatPhongBUS datBUS = new DatPhongBUS();
        DichVuBUS dvBUS = new DichVuBUS();

        public FormDichVu()
        {
            Text = "Gọi dịch vụ";
            Size = new Size(500, 300);
            StartPosition = FormStartPosition.CenterScreen;

            ThemLabel("Phiếu đang ở:", 25, 30);
            cboPhieu.SetBounds(150, 30, 280, 28);
            cboPhieu.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboPhieu);

            ThemLabel("Dịch vụ:", 25, 80);
            cboDichVu.SetBounds(150, 80, 280, 28);
            cboDichVu.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboDichVu);

            ThemLabel("Số lượng:", 25, 130);
            numSoLuong.SetBounds(150, 130, 120, 28);
            numSoLuong.Minimum = 1;
            numSoLuong.Maximum = 10000;
            Controls.Add(numSoLuong);

            Button btnLuu = new Button();
            btnLuu.Text = "Gọi dịch vụ";
            btnLuu.SetBounds(150, 185, 130, 35);
            btnLuu.Click += (s, e) => LuuDichVu();
            Controls.Add(btnLuu);

            TaiDuLieu();
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

        void TaiDuLieu()
        {
            try
            {
                DataTable dtPhieu = datBUS.LayPhieuDangO();
                cboPhieu.DataSource = dtPhieu;
                cboPhieu.DisplayMember = "MaPhieu";
                cboPhieu.ValueMember = "MaPhieu";

                DataTable dtDV = dvBUS.LayDanhSach();
                cboDichVu.DataSource = dtDV;
                cboDichVu.DisplayMember = "TenDV";
                cboDichVu.ValueMember = "MaDV";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        void LuuDichVu()
        {
            try
            {
                if (cboPhieu.SelectedValue == null ||
                    cboDichVu.SelectedValue == null)
                    throw new Exception("Hãy chọn phiếu và dịch vụ.");

                datBUS.ThemDichVu(
                    Convert.ToInt32(cboPhieu.SelectedValue),
                    Convert.ToInt32(cboDichVu.SelectedValue),
                    Convert.ToInt32(numSoLuong.Value)
                );

                MessageBox.Show("Đã lưu dịch vụ.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gọi dịch vụ: " + ex.Message);
            }
        }
    }
}
