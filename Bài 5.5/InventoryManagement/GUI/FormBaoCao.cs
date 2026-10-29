
using System;
using System.Drawing;
using System.Windows.Forms;
using InventoryManagement.BUS;

namespace InventoryManagement.GUI
{
    public class FormBaoCao : Form
    {
        NumericUpDown numNguong = new NumericUpDown();
        DataGridView dgv = new DataGridView();
        Label lblTong = new Label();

        BaoCaoBUS bus = new BaoCaoBUS();

        public FormBaoCao()
        {
            Text = "Báo cáo tồn kho";
            Size = new Size(850, 520);
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();
        }

        void TaoGiaoDien()
        {
            Label lb = new Label();
            lb.Text = "Ngưỡng cảnh báo:";
            lb.SetBounds(20, 20, 160, 30);
            Controls.Add(lb);

            numNguong.SetBounds(180, 20, 120, 30);
            numNguong.Minimum = 0;
            numNguong.Maximum = 100000000;
            numNguong.Value = 10;
            Controls.Add(numNguong);

            Button btnLoc = new Button();
            btnLoc.Text = "Lọc vật tư sắp hết";
            btnLoc.SetBounds(320, 18, 170, 35);
            btnLoc.Click += (s, e) => TaiCanhBao();
            Controls.Add(btnLoc);

            Button btnTong = new Button();
            btnTong.Text = "Tính tổng giá trị tồn";
            btnTong.SetBounds(510, 18, 230, 35);
            btnTong.Click += (s, e) => TinhTongGiaTri();
            Controls.Add(btnTong);

            dgv.SetBounds(20, 75, 790, 320);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgv);

            lblTong.Text = "Tổng giá trị tồn kho: Chưa tính";
            lblTong.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTong.SetBounds(20, 410, 790, 35);
            Controls.Add(lblTong);
        }

        void TaiCanhBao()
        {
            try
            {
                dgv.DataSource = bus.CanhBaoTonKho(numNguong.Value);

                if (dgv.Rows.Count == 0)
                    MessageBox.Show("Không có vật tư nào dưới ngưỡng cảnh báo.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải cảnh báo: " + ex.Message);
            }
        }

        void TinhTongGiaTri()
        {
            try
            {
                decimal tong = bus.TongGiaTriTonKho();
                lblTong.Text = "Tổng giá trị tồn kho: " +
                    tong.ToString("N2") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính tổng giá trị: " + ex.Message);
            }
        }
    }
}
