using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class DonHangRepositories : IDonHangRepositories
    {
        private readonly ApplicationDbContext _db;

        public DonHangRepositories(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<DonHang> CreateAsync(DonHang donHang)
        {
            await _db.DonHang.AddAsync(donHang);
            await _db.SaveChangesAsync();
            return donHang;
        }

        public async Task<DonHang?> DeleteAsync(string id)
        {
            var existingDonHang = await _db.DonHang.FirstOrDefaultAsync(x => x.MaDonHang == id);
            existingDonHang.TinhTrang = "Đã hủy thanh toán";
            if (existingDonHang != null)
            {
                _db.DonHang.Update(existingDonHang);
                await _db.SaveChangesAsync();
                return existingDonHang;
            }
            return null;
        }

        public async Task<IEnumerable<DonHang>> GetAllAsync()
        {
            return await _db.DonHang.ToListAsync();
        }

        public async Task<IEnumerable<ChiTietDonHang>> GetChiTietDonHangByMaDonHang(string maDonHang)
        {
            return await _db.ChiTietDonHang
                .Include(ct => ct.SanPham) // Đảm bảo lấy cả thông tin sản phẩm
                .Where(ct => ct.MaDonHang == maDonHang)
                .ToListAsync();
        }

        public async Task<DonHang?> GetDonHangById(string id)
        {
            return await _db.DonHang.FirstOrDefaultAsync(x => x.MaDonHang == id);
        }

        public async Task<IEnumerable<DonHang>> GetDonHangByKhachHang(string maKhachHang)
        {
            return await _db.DonHang
                            .Include(d => d.ChiTietDonHang)
                            .Where(d => d.MaKhachHang == maKhachHang)
                            .ToListAsync();
        }


        public async Task<IEnumerable<DonHang>> GetLichSuMuaHangByKhachHang(string maKhachHang)
        {
            return await _db.DonHang
                .Where(dh => dh.MaKhachHang == maKhachHang)
                .ToListAsync();
        }

        public async Task<SanPham> GetSanPhamById(string id)
        {
            return await _db.SanPham.FindAsync(id);
        }

        public async Task<DonHang?> UpdateAsync(DonHang donHang)
        {
            var existingDonHang = await _db.DonHang.FirstOrDefaultAsync(x => x.MaDonHang == donHang.MaDonHang);
            if (existingDonHang == null)
            {
                return null;
            }
            _db.Entry(existingDonHang).CurrentValues.SetValues(donHang);
            await _db.SaveChangesAsync();
            return donHang;
        }

        public async Task UpdateSanPhamAsync(SanPham sanPham)
        {
            _db.SanPham.Update(sanPham);
            await _db.SaveChangesAsync();
        }
        public byte[] ExportDonHangToExcel()
        {
            var DonHangs = _db.DonHang.ToList();
            if (DonHangs == null) return [];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DonHang");
                // Add header
                worksheet.Cells[1, 1].Value = "Mã đơn hàng";
                worksheet.Cells[1, 2].Value = "Mã khách hàng";
                worksheet.Cells[1, 3].Value = "Thời gian đặt hàng";
                worksheet.Cells[1, 4].Value = "Tổng tiền";
                worksheet.Cells[1, 5].Value = "Địa chỉ giao hàng";
                worksheet.Cells[1, 6].Value = "Thông tin thanh toán";
                worksheet.Cells[1, 7].Value = "Tình trạng";

                // Add data
                for (int i = 0; i < DonHangs.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = DonHangs[i].MaDonHang;
                    worksheet.Cells[i + 2, 2].Value = DonHangs[i].MaKhachHang;
                    worksheet.Cells[i + 2, 3].Value = DonHangs[i].ThoiGianDatHang;
                    worksheet.Cells[i + 2, 3].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
                    worksheet.Cells[i + 2, 4].Value = DonHangs[i].TongTien;
                    worksheet.Cells[i + 2, 5].Value = DonHangs[i].DiaChiGiaoHang;
                    worksheet.Cells[i + 2, 6].Value = DonHangs[i].ThongTinThanhToan;
                    worksheet.Cells[i + 2, 7].Value = DonHangs[i].TinhTrang;
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }

        }
    }
}
