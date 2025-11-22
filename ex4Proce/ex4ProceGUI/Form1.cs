using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using ex4ProceBUS;
using ex4ProceDTO;

namespace QLPhieuNhap_GUI
{
    public partial class Form1 : Form
    {
        NhaCungCapBUS nccBUS = new NhaCungCapBUS();
        SanPhamBUS spBUS = new SanPhamBUS();
        PhieuNhapBUS pnBUS = new PhieuNhapBUS();
        ChiTietBUS ctBUS = new ChiTietBUS();

        private string maSpDangSua = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadComboBoxNhaCungCap();
            LoadComboBoxSanPham();
            LoadComboBoxPhieuNhap();

            LoadDataGridView();

            groupBoxChiTiet.Enabled = false;    
            txtDonGia.ReadOnly = true;          
            txtThanhTien.ReadOnly = true;      

            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.ContextMenuStrip = contextMenuStrip1;
        }


        private void LoadComboBoxNhaCungCap()
        {
            cboNhaCungCap.DataSource = nccBUS.LayDanhSach();
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "MaNCC";
        }

        private void LoadComboBoxSanPham()
        {
            cboMaPN_ChiTiet.DataSource = pnBUS.LayDanhSach();
            cboMaPN_ChiTiet.DisplayMember = "MaPN";
            cboMaPN_ChiTiet.ValueMember = "MaPN";
        }

        private void LoadComboBoxPhieuNhap()
        {
            cboMaPN_ChiTiet.DataSource = pnBUS.LayDanhSach();
            cboMaPN_ChiTiet.DisplayMember = "MaPN";
            cboMaPN_ChiTiet.ValueMember = "MaPN";
        }

        private void LoadDataGridView()
        {
            dgvChiTiet.DataSource = ctBUS.LayDanhSachChiTiet();

            if (dgvChiTiet.Columns["DonGia"] != null)
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0"; 

            dgvChiTiet.Columns["MaPN"].HeaderText = "Mã Phiếu";
            dgvChiTiet.Columns["MaSP"].HeaderText = "Mã SP";
            dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
        }

        private void UpdateTongTien(string maPN)
        {
            decimal tong = ctBUS.TinhTongTien(maPN);
            txtThanhTien.Text = tong.ToString("N0") + " VNĐ";
        }

        private void ResetChiTietControls()
        {
            cboMaSanPham.SelectedIndex = -1;
            txtSoLuong.Text = "1";
            txtDonGia.Text = "";

            maSpDangSua = null;
            btnThemSanPham.Text = "Thêm sản phẩm";

            cboMaSanPham.Enabled = true;
            cboMaPN_ChiTiet.Enabled = true; 
        }


        private void btnLuuPhieuNhap_Click(object sender, EventArgs e)
        {
            string maPN = txtMaPhieuNhap.Text.Trim();
            string maNCC = cboNhaCungCap.SelectedValue?.ToString();
            DateTime ngayNhap = dtpNgayNhap.Value;

            string ketQua = pnBUS.ThemPhieuNhap(maPN, maNCC, ngayNhap);

            if (ketQua == null)
            {
                MessageBox.Show("Lưu phiếu nhập thành công!");

                LoadComboBoxPhieuNhap();

                cboMaPN_ChiTiet.SelectedValue = maPN;
            }
            else
            {
                MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaoPhieuNhap_Click(object sender, EventArgs e)
        {
            string maPN_tren = txtMaPhieuNhap.Text.Trim();

            groupBoxPhieuNhap.Enabled = true;
            groupBoxChiTiet.Enabled = true;

            LoadComboBoxPhieuNhap();

            if (!string.IsNullOrEmpty(maPN_tren) && cboMaPN_ChiTiet.FindStringExact(maPN_tren) != -1)
            {
                cboMaPN_ChiTiet.SelectedValue = maPN_tren;
            }
            else
            {
                if (cboMaPN_ChiTiet.Items.Count > 0)
                {
                    cboMaPN_ChiTiet.SelectedIndex = 0;
                }
            }
            cboMaPN_ChiTiet.Enabled = true;
        }



        private void cboMaSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaSanPham.SelectedValue == null) return;

            if (cboMaSanPham.SelectedValue is string maSP)
            {
                
                SanPhamDTO sp = spBUS.LayTheoMa(maSP);
                if (sp != null && sp.DonGia.HasValue)
                {
                    txtDonGia.Text = sp.DonGia.Value.ToString("N0");
                }
                else
                {
                    txtDonGia.Text = "0";
                }
            }
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
        
            if (cboMaPN_ChiTiet.SelectedValue == null || cboMaSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập và sản phẩm.");
                return;
            }

            string maPN = cboMaPN_ChiTiet.SelectedValue.ToString();
            string maSP = cboMaSanPham.SelectedValue.ToString();

            int soLuong = 0;
            if (!int.TryParse(txtSoLuong.Text, out soLuong))
            {
                MessageBox.Show("Số lượng phải là số.");
                return;
            }

            decimal donGia = 0;
            string strDonGia = txtDonGia.Text.Replace(",", "").Replace(".", "").Replace(" VNĐ", "").Trim();
            decimal.TryParse(strDonGia, out donGia);

          
            bool cheDoSua = (maSpDangSua != null);

            string ketQua = ctBUS.ThemHoacCapNhat(maPN, maSP, soLuong, donGia, cheDoSua);

            if (ketQua == null)
            {
                LoadDataGridView();   
                UpdateTongTien(maPN);   
                ResetChiTietControls();  

                if (cheDoSua) MessageBox.Show("Cập nhật thành công!");
                else MessageBox.Show("Thêm thành công!");
            }
            else
            {
                MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboMaPN_ChiTiet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaPN_ChiTiet.SelectedValue is string maPN)
            {
                UpdateTongTien(maPN);
            }
        }



        private void dgvChiTiet_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvChiTiet.CurrentCell = dgvChiTiet.Rows[e.RowIndex].Cells[1];
                dgvChiTiet.Rows[e.RowIndex].Selected = true;
            }
        }

        private void menuSua_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null) return;

            string maPN = dgvChiTiet.CurrentRow.Cells["MaPN"].Value.ToString();
            string maSP = dgvChiTiet.CurrentRow.Cells["MaSP"].Value.ToString();
            string soLuong = dgvChiTiet.CurrentRow.Cells["SoLuong"].Value.ToString();

            string donGiaStr = dgvChiTiet.CurrentRow.Cells["DonGia"].Value.ToString();

            cboMaPN_ChiTiet.SelectedValue = maPN;
            cboMaSanPham.SelectedValue = maSP;
            txtSoLuong.Text = soLuong;

            decimal donGiaVal = 0;
            if (decimal.TryParse(donGiaStr.Replace(",", "").Replace(".", ""), out donGiaVal))
            {
                txtDonGia.Text = donGiaVal.ToString("N0");
            }
            else
            {
                txtDonGia.Text = donGiaStr;
            }

            maSpDangSua = maSP;       
            btnThemSanPham.Text = "Cập nhật";

            cboMaPN_ChiTiet.Enabled = false;
            cboMaSanPham.Enabled = false;
        }

        private void menuXoa_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null) return;

            string maPN = dgvChiTiet.CurrentRow.Cells["MaPN"].Value.ToString();
            string maSP = dgvChiTiet.CurrentRow.Cells["MaSP"].Value.ToString();

            var confirm = MessageBox.Show($"Bạn muốn xóa sản phẩm {maSP} khỏi phiếu {maPN}?",
                                          "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                string ketQua = ctBUS.XoaChiTiet(maPN, maSP);
                if (ketQua == null)
                {
                    LoadDataGridView();
                    UpdateTongTien(maPN);
                    MessageBox.Show("Đã xóa!");
                }
                else
                {
                    MessageBox.Show(ketQua, "Lỗi");
                }
            }
        }
               private void btnIn_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có dữ liệu để in.", "Thông báo");
                return;
            }

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(InPhieu_PrintPage);

            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = pd;
            dlg.Width = 700;
            dlg.Height = 800;
            dlg.StartPosition = FormStartPosition.CenterScreen;

            dlg.ShowDialog();
        }

        private void InPhieu_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font fontTieuDe = new Font("Arial", 20, FontStyle.Bold);
            Font fontHeader = new Font("Arial", 11, FontStyle.Bold);
            Font fontNoiDung = new Font("Arial", 11, FontStyle.Regular);

            float x = 50;
            float y = 50;
            float width = e.PageBounds.Width - 100;


            string tieuDe = "PHIẾU NHẬP HÀNG";
            SizeF sizeTieuDe = e.Graphics.MeasureString(tieuDe, fontTieuDe);
            float xTieuDe = (e.PageBounds.Width - sizeTieuDe.Width) / 2;
            e.Graphics.DrawString(tieuDe, fontTieuDe, Brushes.Black, xTieuDe, y);

            y += 50;

            string ngayThang = dtpNgayNhap.Value.ToString("dd/MM/yyyy");
            string thongTin = $"Mã phiếu: {txtMaPhieuNhap.Text}   |   Ngày: {ngayThang}";
            e.Graphics.DrawString(thongTin, fontNoiDung, Brushes.Black, x, y);

            y += 40;

            float col1 = x;
            float col2 = x + 120;
            float col3 = x + 240;
            float col4 = x + 350;

            e.Graphics.DrawString("Mã SP", fontHeader, Brushes.Black, col1, y);
            e.Graphics.DrawString("Mã PN", fontHeader, Brushes.Black, col2, y);
            e.Graphics.DrawString("SL", fontHeader, Brushes.Black, col3, y);
            e.Graphics.DrawString("Đơn Giá", fontHeader, Brushes.Black, col4, y);

            y += 25;
            e.Graphics.DrawLine(new Pen(Brushes.Black, 2), x, y, x + 500, y);
            y += 10;

            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue;

                string maSP = row.Cells["MaSP"].Value?.ToString() ?? "";
                string maPN = row.Cells["MaPN"].Value?.ToString() ?? "";
                string sl = row.Cells["SoLuong"].Value?.ToString() ?? "0";

                decimal valGia = 0;
                if (row.Cells["DonGia"].Value != null)
                    decimal.TryParse(row.Cells["DonGia"].Value.ToString(), out valGia);
                string gia = valGia.ToString("N0");

                e.Graphics.DrawString(maSP, fontNoiDung, Brushes.Black, col1, y);
                e.Graphics.DrawString(maPN, fontNoiDung, Brushes.Black, col2, y);
                e.Graphics.DrawString(sl, fontNoiDung, Brushes.Black, col3, y);
                e.Graphics.DrawString(gia, fontNoiDung, Brushes.Black, col4, y);

                y += 30;
            }

            y += 10;
            e.Graphics.DrawLine(new Pen(Brushes.Black, 2), x, y, x + 500, y);
            y += 20;

            string tongTienText = $"TỔNG TIỀN: {txtThanhTien.Text}";
            SizeF sizeTong = e.Graphics.MeasureString(tongTienText, fontHeader);
            float xTong = (e.PageBounds.Width - sizeTong.Width) / 2;

            e.Graphics.DrawString(tongTienText, fontHeader, Brushes.Black, xTong, y);
        }
    }
    }