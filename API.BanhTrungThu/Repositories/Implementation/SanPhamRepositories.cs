using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class SanPhamRepositories:ISanPhamRepositories
    {
        private readonly ApplicationDbContext _db;

        public SanPhamRepositories(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<SanPham> CreateAsync(SanPham sanPham)
        {
            await _db.SanPham.AddAsync(sanPham);
            await _db.SaveChangesAsync();
            return sanPham;
        }

        public async Task<SanPham?> DeleteAsync(string id)
        {
            var existingSanPham = await _db.SanPham.FirstOrDefaultAsync(x => x.MaSanPham == id);
            existingSanPham.TinhTrang = "Ngưng bán";
            if (existingSanPham != null)
            {
                _db.SanPham.Update(existingSanPham);
                await _db.SaveChangesAsync();
                return existingSanPham;
            }
            return null;
        }

        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _db.SanPham.ToListAsync();
        }

        public async Task<SanPham?> GetSanPhamById(string id)
        {
            return await _db.SanPham.FirstOrDefaultAsync(x => x.MaSanPham == id);
        }

        public async Task<SanPham?> UpdateAsync(SanPham sanPham)
        {
            var existingSanPham = await _db.SanPham.Include(x => x.MaLoai).FirstOrDefaultAsync(x => x.MaSanPham == sanPham.MaSanPham);
            if (existingSanPham == null)
            {
                return null;
            }

            _db.Entry(existingSanPham).CurrentValues.SetValues(sanPham);

            existingSanPham.MaLoai = sanPham.MaLoai;

            await _db.SaveChangesAsync();
            return sanPham;
        }
    }
}
