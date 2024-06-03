using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "cd4bc7e9-897b-4c15-846c-e76c2aaee206";
            var khachHangRoleId = "9d8c4570-1e1e-43c0-8214-0ea0146a6f09";
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole()
                {
                    Id = khachHangRoleId,
                    Name = "Khách hàng",
                    NormalizedName = "Khách hàng".ToUpper(),
                    ConcurrencyStamp = khachHangRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
            //Tạo admin
            var adminUserId = "4106353f-cbc4-4192-8d34-6d9bcf98802f";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "admin@gmail.com".ToUpper()
            };
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "admin123");
            //Tạo khách hàng
            var khachHangId = "90b2ba0b-c552-44f6-bf4d-cc46fa5731b5";
            var khachHang = new IdentityUser()
            {
                Id = khachHangId,
                UserName = "Khách hàng",
                Email = "khachhang@gmail.com",
                NormalizedEmail = "khachhang@gmail.com".ToUpper(),
                NormalizedUserName = "khachhang@gmail.com".ToUpper()
            };
            khachHang.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(khachHang, "khachhang123");
            builder.Entity<IdentityUser>().HasData(admin);
            builder.Entity<IdentityUser>().HasData(khachHang);
            // Trao vai trò cho quản trị viên
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                },
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

            var khachHangRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = khachHangId,
                    RoleId = khachHangRoleId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(khachHangRoles);
        }
    }
}
