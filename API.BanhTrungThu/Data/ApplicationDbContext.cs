using API.BanhTrungThu.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<LoaiSanPham> LoaiSanPham { get;set; }
        public DbSet<SanPham> SanPham { get;set; }
        public DbSet<AnhSanPham> AnhSanPham { get;set; }
        public DbSet<KhachHang> KhachHang { get;set; }
        public DbSet<DonHang> DonHang { get;set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get;set; }
        public DbSet<DanhGia>DanhGia {  get;set; } 
    }
}
