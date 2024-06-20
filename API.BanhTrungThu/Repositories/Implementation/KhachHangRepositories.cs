using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class KhachHangRepositories : IKhachHangRepositories
    {
        private readonly ApplicationDbContext _db;

        public KhachHangRepositories(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<KhachHang> CreateAsync(KhachHang khachHang)
        {
            await _db.KhachHang.AddAsync(khachHang);
            await _db.SaveChangesAsync();
            return khachHang;
        }

        public async Task<KhachHang?> DeleteAsync(string id)
        {
            var existingKhachHang = await _db.KhachHang.FirstOrDefaultAsync(x => x.MaKhachHang == id);
            existingKhachHang.TinhTrang = "Ngưng hoạt động";
            if (existingKhachHang != null)
            {
                _db.KhachHang.Update(existingKhachHang);
                await _db.SaveChangesAsync();
                return existingKhachHang;
            }
           
            return null;
        }

        public async Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            return await _db.KhachHang.ToListAsync();
        }

        public async Task<KhachHang?> GetKhachHangById(string id)
        {
            return await _db.KhachHang.FirstOrDefaultAsync(x => x.MaKhachHang == id);
        }

        public async Task<KhachHang?> UpdateAsync(KhachHang khachHang)
        {
            var existingKhachHang = await _db.KhachHang.FirstOrDefaultAsync(x => x.MaKhachHang == khachHang.MaKhachHang);
            if (existingKhachHang != null)
            {
                _db.Entry(existingKhachHang).CurrentValues.SetValues(khachHang);
                await _db.SaveChangesAsync();
                return khachHang;
            }
            return null;
        }

        public byte[] ExportKhachHangToExcel()
        {
            var KhachHangs = _db.KhachHang.ToList();
            if (KhachHangs == null) return [];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("KhachHang");
                // Add header
                worksheet.Cells[1, 1].Value = "Mã khách hàng";
                worksheet.Cells[1, 2].Value = "Tên khách hàng";
                worksheet.Cells[1, 3].Value = "Số Điện Thoại";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "Địa Chỉ";
                worksheet.Cells[1, 6].Value = "Tình trạng";
                worksheet.Cells[1, 7].Value = "Ngày đăng ký";

                // Add data
                for (int i = 0; i < KhachHangs.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = KhachHangs[i].MaKhachHang;
                    worksheet.Cells[i + 2, 2].Value = KhachHangs[i].TenKhachHang;
                    worksheet.Cells[i + 2, 3].Value = KhachHangs[i].SoDienThoai;
                    worksheet.Cells[i + 2, 4].Value = KhachHangs[i].Email;
                    worksheet.Cells[i + 2, 5].Value = KhachHangs[i].DiaChi;
                    worksheet.Cells[i + 2, 6].Value = KhachHangs[i].TinhTrang;
                    worksheet.Cells[i + 2, 7].Value = KhachHangs[i].NgayDangKy;
                    worksheet.Cells[i + 2, 7].Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }

        }
    }
}
