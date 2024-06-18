﻿// <auto-generated />
using System;
using API.BanhTrungThu.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.BanhTrungThu.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.AnhSanPham", b =>
                {
                    b.Property<int>("MaAnh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaAnh"));

                    b.Property<string>("MaSanPham")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("TenAnh")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MaAnh");

                    b.HasIndex("MaSanPham");

                    b.ToTable("AnhSanPham");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.ChiTietDonHang", b =>
                {
                    b.Property<string>("MaChiTiet")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<double>("Gia")
                        .HasColumnType("float");

                    b.Property<string>("MaDonHang")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("MaSanPham")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaChiTiet");

                    b.HasIndex("MaDonHang");

                    b.HasIndex("MaSanPham");

                    b.ToTable("ChiTietDonHang");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.DanhGia", b =>
                {
                    b.Property<int>("MaDanhGia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDanhGia"));

                    b.Property<string>("BinhLuan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DiemDanhGia")
                        .HasColumnType("int");

                    b.Property<string>("MaKhachHang")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("MaSanPham")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime>("NgayDanhGia")
                        .HasColumnType("datetime2");

                    b.HasKey("MaDanhGia");

                    b.HasIndex("MaKhachHang");

                    b.HasIndex("MaSanPham");

                    b.ToTable("DanhGia");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.DonHang", b =>
                {
                    b.Property<string>("MaDonHang")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("DiaChiGiaoHang")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("MaKhachHang")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime?>("ThoiGianDatHang")
                        .HasColumnType("datetime2");

                    b.Property<string>("ThongTinThanhToan")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("TongTien")
                        .HasColumnType("float");

                    b.HasKey("MaDonHang");

                    b.HasIndex("MaKhachHang");

                    b.ToTable("DonHang");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.KhachHang", b =>
                {
                    b.Property<string>("MaKhachHang")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime>("NgayDangKy")
                        .HasColumnType("Date");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("TenKhachHang")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaKhachHang");

                    b.ToTable("KhachHang");

                    b.HasData(
                        new
                        {
                            MaKhachHang = "KH0001",
                            DiaChi = "Buôn Ma Thuột, ĐakLak",
                            Email = "admin@gmail.com",
                            NgayDangKy = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SoDienThoai = "0123123123",
                            TenKhachHang = "ADMIN",
                            TinhTrang = "Đang hoạt động"
                        });
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.LoaiSanPham", b =>
                {
                    b.Property<string>("MaLoai")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("AnhLoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenLoai")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("MaLoai");

                    b.ToTable("LoaiSanPham");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.SanPham", b =>
                {
                    b.Property<string>("MaSanPham")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<double>("Gia")
                        .HasColumnType("float");

                    b.Property<string>("MaLoai")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("NgayThem")
                        .HasColumnType("datetime2");

                    b.Property<int>("SoLuongTrongKho")
                        .HasColumnType("int");

                    b.Property<string>("TenSanPham")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaSanPham");

                    b.HasIndex("MaLoai");

                    b.ToTable("SanPham");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.AnhSanPham", b =>
                {
                    b.HasOne("API.BanhTrungThu.Models.Domain.SanPham", "SanPham")
                        .WithMany()
                        .HasForeignKey("MaSanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.ChiTietDonHang", b =>
                {
                    b.HasOne("API.BanhTrungThu.Models.Domain.DonHang", "DonHang")
                        .WithMany("ChiTietDonHang")
                        .HasForeignKey("MaDonHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.BanhTrungThu.Models.Domain.SanPham", "SanPham")
                        .WithMany()
                        .HasForeignKey("MaSanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DonHang");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.DanhGia", b =>
                {
                    b.HasOne("API.BanhTrungThu.Models.Domain.KhachHang", "KhachHang")
                        .WithMany()
                        .HasForeignKey("MaKhachHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.BanhTrungThu.Models.Domain.SanPham", "SanPham")
                        .WithMany()
                        .HasForeignKey("MaSanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.DonHang", b =>
                {
                    b.HasOne("API.BanhTrungThu.Models.Domain.KhachHang", "KhachHang")
                        .WithMany()
                        .HasForeignKey("MaKhachHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.SanPham", b =>
                {
                    b.HasOne("API.BanhTrungThu.Models.Domain.LoaiSanPham", "LoaiSanPham")
                        .WithMany()
                        .HasForeignKey("MaLoai")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoaiSanPham");
                });

            modelBuilder.Entity("API.BanhTrungThu.Models.Domain.DonHang", b =>
                {
                    b.Navigation("ChiTietDonHang");
                });
#pragma warning restore 612, 618
        }
    }
}
