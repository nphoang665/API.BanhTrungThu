using API.BanhTrungThu.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<AnhSanPham> AnhSanPham { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<DanhGia> DanhGia { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu mẫu
            modelBuilder.Entity<KhachHang>().HasData(
                new KhachHang
                {
                    MaKhachHang = "KH0001",
                    TenKhachHang = "ADMIN",
                    SoDienThoai = "0123123123",
                    Email = "admin@gmail.com",
                    DiaChi = "Buôn Ma Thuột, ĐakLak",
                    TinhTrang = "Đang hoạt động",
                    NgayDangKy = new DateTime(2024, 1, 1),
                }
            );
        }
    }
}
