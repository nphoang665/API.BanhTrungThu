﻿using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class LoaiSanPhamRepositories : ILoaiSanPhamRepositories
    {
        private readonly ApplicationDbContext _db;
        public LoaiSanPhamRepositories(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<LoaiSanPham> CreateAsync(LoaiSanPham loaiSanPham)
        {
            await _db.LoaiSanPham.AddAsync(loaiSanPham);
            await _db.SaveChangesAsync();
            return loaiSanPham;
        }

        public async Task<LoaiSanPham?> DeleteAsync(string id)
        {
            var existingLoaiSanPham = await _db.LoaiSanPham.FirstOrDefaultAsync(x => x.MaLoai == id);

            if(existingLoaiSanPham is null)
            {
                return null;
            }
            _db.LoaiSanPham.Remove(existingLoaiSanPham);
            await _db.SaveChangesAsync();
            return existingLoaiSanPham;
        }

        public async Task<IEnumerable<LoaiSanPham>> GetAllAsync()
        {
            return await _db.LoaiSanPham.ToListAsync();
        }

        public async Task<LoaiSanPham?> GetLoaiSanPhamById(string id)
        {
            return await _db.LoaiSanPham.FirstOrDefaultAsync(x => x.MaLoai == id);
        }

        public async Task<LoaiSanPham?> UpdateAsync(LoaiSanPham loaiSanPham)
        {
            var existingLoaiSanPham = await _db.LoaiSanPham.FirstOrDefaultAsync(x => x.MaLoai == loaiSanPham.MaLoai);
            if (existingLoaiSanPham != null)
            {
                _db.Entry(existingLoaiSanPham).CurrentValues.SetValues(loaiSanPham);
                await _db.SaveChangesAsync();
                return loaiSanPham;
            }
            return null;
        }
    }
}
