
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.BUS;

namespace HotelManagement.GUI
{
    public class FormCheckOut : Form
    {
        ComboBox cboPhieu = new ComboBox();
        Label lblHoaDon = new Label();
        DatPhongBUS bus = new DatPhongBUS();

        public FormCheckOut()
        {
            Text = "Check-out - Thanh toán";
            Size = new Size(520, 280);
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(new Label
            {
                Text = "Chọn phiếu đang lưu trú:",
                Left = 25,
                Top = 30,
                Width = 180,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            });

            cboPhieu.SetBounds(210, 30, 250, 30);
            cboPhieu.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboPhieu);

            Button btnTai = new Button();
            btnTai.Text = "Tải lại";
            btnTai.SetBounds(210, 75, 100, 32);
            btnTai.Click += (s, e) => TaiPhieu();
            Controls.Add(btnTai);

            Button btnThanhToan = new Button();
            btnThanhToan.Text = "Xác nhận thanh toán";
            btnThanhToan.SetBounds(210, 120, 180, 35);
            btnThanhToan.Click += (s, e) => ThanhToan();
            Controls.Add(btnThanhToan);

            lblHoaDon.SetBounds(25, 175, 440, 35);
            lblHoaDon.Font = new Font("Arial", 11, FontStyle.Bold);
            Controls.Add(lblHoaDon);

            TaiPhieu();
        }

        void TaiPhieu()
        {
            try
            {
                cboPhieu.DataSource = bus.LayPhieuDangO();
                cboPhieu.DisplayMember = "MaPhieu";
                cboPhieu.ValueMember = "MaPhieu";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phiếu: " + ex.Message);
            }
        }

        void ThanhToan()
        {
            try
            {
                if (cboPhieu.SelectedValue == null)
                    throw new Exception("Không có phiếu đang lưu trú.");

                int maPhieu = Convert.ToInt32(cboPhieu.SelectedValue);

                DialogResult chon = MessageBox.Show(
                    "Bạn xác nhận thanh toán phiếu " + maPhieu + "?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (chon != DialogResult.Yes) return;

                decimal tongTien = bus.CheckOut(maPhieu);
                lblHoaDon.Text = "Tổng thanh toán: " + tongTien.ToString("N2") + " VNĐ";

                MessageBox.Show("Check-out thành công. Phòng chuyển sang trạng thái Đang dọn dẹp.");
                TaiPhieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi check-out: " + ex.Message);
            }
        }
    }
}
