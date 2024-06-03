using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class ChiTietDonHangRepositories : IChiTietDonHangRepositories
    {
        private readonly ApplicationDbContext _db;

        public ChiTietDonHangRepositories(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ChiTietDonHang> CreateAsync(ChiTietDonHang chiTietDonHang)
        {
            await _db.ChiTietDonHang.AddAsync(chiTietDonHang);
            await _db.SaveChangesAsync();
            return chiTietDonHang;
        }

        public Task<ChiTietDonHang?> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ChiTietDonHang>> GetAllAsync()
        {
            return await _db.ChiTietDonHang.Include(x => x.DonHang).Include(x => x.SanPham).ToListAsync();
        }

        public async Task<IEnumerable<ChiTietDonHang?>> GetDonHangById(string id)
        {
            return await _db.ChiTietDonHang.Where(s => s.MaChiTiet == id).Include(x => x.DonHang).ToListAsync();
        }

        public async Task<ChiTietDonHang?> UpdateAsync(ChiTietDonHang chiTietDonHang)
        {
            var existingChiTietDonHang = await _db.ChiTietDonHang.FirstOrDefaultAsync(x => x.MaChiTiet == chiTietDonHang.MaChiTiet);
            if (existingChiTietDonHang == null)
            {
                return null;
            }
            _db.Entry(existingChiTietDonHang).CurrentValues.SetValues(chiTietDonHang);
            await _db.SaveChangesAsync();
            return chiTietDonHang;
        }
    }
}
