using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class DanhGiaRepository : IDanhGiaRepository
    {
        private readonly ApplicationDbContext _db;

        public DanhGiaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DanhGia> AddDanhGia(DanhGia danhGia)
        {
            _db.DanhGia.Add(danhGia);
            await _db.SaveChangesAsync();
            return danhGia;
        }

        public async Task<IEnumerable<DanhGia>> GetDanhGiaBySanPham(string maSanPham)
        {
            return await _db.DanhGia
               .Where(dg => dg.MaSanPham == maSanPham)
               .ToListAsync();
        }
    }
}
