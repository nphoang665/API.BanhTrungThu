using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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
            if(existingDonHang != null)
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

        public async Task<DonHang?> GetDonHangById(string id)
        {
            return await _db.DonHang.FirstOrDefaultAsync(x => x.MaDonHang == id);
        }

        public async Task<DonHang?> UpdateAsync(DonHang donHang)
        {
            var existingDonHang = await _db.DonHang.FirstOrDefaultAsync(x => x.MaDonHang == donHang.MaDonHang);
            if(existingDonHang == null)
            {
                return null;
            }
            _db.Entry(existingDonHang).CurrentValues.SetValues(donHang);
            await _db.SaveChangesAsync();
            return donHang;
        }
    }
}
