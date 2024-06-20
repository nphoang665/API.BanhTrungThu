using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class LoaiSanPhamRepositories : ILoaiSanPhamRepositories
    {
        private readonly ApplicationDbContext _db;
        public LoaiSanPhamRepositories(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<LoaiSanPham> CreateAsync(LoaiSanPham loaiSanPham)
        {
            await _db.LoaiSanPham.AddAsync(loaiSanPham);
            await _db.SaveChangesAsync();
            return loaiSanPham;
        }

        public async Task<LoaiSanPham?> DeleteAsync(string id)
        {
            var existingLoaiSanPham = await _db.LoaiSanPham.FirstOrDefaultAsync(x => x.MaLoai == id);

            if(existingLoaiSanPham is null)
            {
                return null;
            }
            var hasProducts = await _db.SanPham.AnyAsync(x => x.MaLoai == id);
            if (hasProducts)
            {
                // Trả về null hoặc một thông báo lỗi khác để báo rằng loại sản phẩm không thể xóa
                return null;
            }

            _db.LoaiSanPham.Remove(existingLoaiSanPham);
            await _db.SaveChangesAsync();
            return existingLoaiSanPham;  
        }


        public async Task<IEnumerable<LoaiSanPham>> GetAllAsync()
        {
            return await _db.LoaiSanPham.ToListAsync();
        }

        public async Task<LoaiSanPham> GetByIdAsync(string id)
        {
            return await _db.LoaiSanPham.FindAsync(id);
        }

        public async Task<LoaiSanPham?> GetLoaiSanPhamById(string id)
        {
            return await _db.LoaiSanPham.FirstOrDefaultAsync(x => x.MaLoai == id);
        }

        public async Task<LoaiSanPham?> UpdateAsync(LoaiSanPham loaiSanPham)
        {
            var existingLoaiSanPham = await _db.LoaiSanPham.FirstOrDefaultAsync(x => x.MaLoai == loaiSanPham.MaLoai);
            if (existingLoaiSanPham != null)
            {
                _db.Entry(existingLoaiSanPham).CurrentValues.SetValues(loaiSanPham);
                await _db.SaveChangesAsync();
                return loaiSanPham;
            }
            return null;
        }
        public byte[] ExportLoaiSanPhamToExcel()
        {
            var LoaiSanPhams = _db.LoaiSanPham.ToList();
            if (LoaiSanPhams == null) return [];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("LoaiSanPham");
                // Add header
                worksheet.Cells[1, 1].Value = "Mã loại";
                worksheet.Cells[1, 2].Value = "Tên loại";
                worksheet.Cells[1, 3].Value = "Ảnh loại";
               

                // Add data
                for (int i = 0; i < LoaiSanPhams.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = LoaiSanPhams[i].MaLoai;
                    worksheet.Cells[i + 2, 2].Value = LoaiSanPhams[i].TenLoai;
                    worksheet.Cells[i + 2, 3].Value = LoaiSanPhams[i].AnhLoai;                  
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }

        }

    }
}
