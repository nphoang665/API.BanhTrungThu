using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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
    }
}
