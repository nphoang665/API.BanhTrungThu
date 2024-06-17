using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class SanPhamRepositories : ISanPhamRepositories
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

        public async Task<IEnumerable<SanPham>> GetSanPhamBanChayByLoaiAsync(string maLoai)
        {
            var sanPhams = await (from sp in _db.SanPham
                                  join od in _db.ChiTietDonHang on sp.MaSanPham equals od.MaSanPham into spGroup
                                  from sub in spGroup.DefaultIfEmpty()
                                  where sp.MaLoai == maLoai
                                  group sub by new { sp.MaSanPham, sp.TenSanPham, sp.Gia, sp.MaLoai, sp.MoTa, sp.SoLuongTrongKho, sp.NgayThem, sp.TinhTrang } into grouped
                                  orderby grouped.Count() descending
                                  select new SanPham
                                  {
                                      MaSanPham = grouped.Key.MaSanPham,
                                      TenSanPham = grouped.Key.TenSanPham,
                                      Gia = grouped.Key.Gia,
                                      MaLoai = grouped.Key.MaLoai,
                                      MoTa = grouped.Key.MoTa,
                                      SoLuongTrongKho = grouped.Key.SoLuongTrongKho,
                                      NgayThem = grouped.Key.NgayThem,
                                      TinhTrang = grouped.Key.TinhTrang
                                  })
                              .ToListAsync();

            return sanPhams;
        }

        public async Task<SanPham?> GetSanPhamById(string id)
        {
            return await _db.SanPham.FirstOrDefaultAsync(x => x.MaSanPham == id);
        }

        public async Task<IEnumerable<SanPham>> GetSanPhamByLoaiAsync(string maLoai)
        {
            return await _db.SanPham.Where(sp => sp.MaLoai == maLoai).ToListAsync();
        }

        public async Task<IEnumerable<SanPham>> GetSanPhamNoiBatAsync()
        {
            var sanPhams = await (from od in _db.ChiTietDonHang
                                  group od by od.MaSanPham into grouped
                                  orderby grouped.Count() descending
                                  select grouped.Key)
                                  .Take(10)
                                  .Join(_db.SanPham, id => id, sp => sp.MaSanPham, (id, sp) => sp)
                                  .Where(sp => sp.TinhTrang == "Đang hoạt động")
                                  .ToListAsync();

            return sanPhams;
        }


        public async Task<SanPham?> UpdateAsync(SanPham sanPham)
        {
            var existingSanPham = await _db.SanPham.FirstOrDefaultAsync(x => x.MaSanPham == sanPham.MaSanPham);
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
