﻿using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietDonHangController : ControllerBase
    {
        private readonly IChiTietDonHangRepositories _chiTietDonHangRepositories;
        private readonly ISanPhamRepositories _sanPhamRepositories;

        public ChiTietDonHangController(IChiTietDonHangRepositories chiTietDonHangRepositories,ISanPhamRepositories sanPhamRepositories)
        {
            _chiTietDonHangRepositories = chiTietDonHangRepositories;
            _sanPhamRepositories = sanPhamRepositories;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChiTietDonHang()
        {
            var chiTietDonHangs = await _chiTietDonHangRepositories.GetAllAsync();
            var response = new List<ChiTietDonHangDto>();

            foreach(var chiTietDonHang in  chiTietDonHangs)
            {
                response.Add(new ChiTietDonHangDto
                {
                    MaChiTiet = chiTietDonHang.MaChiTiet,
                    MaDonHang = chiTietDonHang.MaDonHang,
                    MaSanPham = chiTietDonHang.MaSanPham,
                    SoLuong = chiTietDonHang.SoLuong,
                    Gia = chiTietDonHang.Gia,
                });
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChiTietDonHang([FromBody] CreateChiTietDonHangRequestDto request)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string id = "CTH" + randomValue.ToString("D3");
            var sanPhams = await _sanPhamRepositories.GetSanPhamById(request.MaSanPham);

            var donHang = new ChiTietDonHang
            {
                MaChiTiet = id,
                MaDonHang = request.MaDonHang,
                MaSanPham = request.MaSanPham,
                SoLuong = request.SoLuong,
                Gia = request.Gia, 
            };
            donHang = await _chiTietDonHangRepositories.CreateAsync(donHang);
            sanPhams.SoLuongTrongKho -= donHang.SoLuong;
            sanPhams = await _sanPhamRepositories.UpdateAsync(sanPhams);

            var response = new ChiTietDonHangDto
            {
                MaChiTiet = donHang.MaChiTiet,
                MaDonHang = donHang.MaDonHang,
                MaSanPham = donHang.MaSanPham,
                SoLuong = donHang.SoLuong,
                Gia = donHang.Gia,
            };

            

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetChiTietDonHangById(string id)
        {
            var chiTietDonHangs = await _chiTietDonHangRepositories.GetDonHangById(id);
            var response = new List<ChiTietDonHangDto>();


            foreach (var chiTietDonHang in chiTietDonHangs)
            {
                response.Add(new ChiTietDonHangDto
                {
                    MaChiTiet = id,
                    MaDonHang = chiTietDonHang.MaDonHang,
                    MaSanPham = chiTietDonHang.MaSanPham,
                    SoLuong = chiTietDonHang.SoLuong,
                    Gia = chiTietDonHang.Gia,
                    
                });
            }
            return Ok(response);
        }

        [HttpGet("donhang/{maDonHang}")]
        public async Task<IActionResult> GetChiTietDonHangByDonHangId(string maDonHang)
        {
            var chiTietDonHangs = await _chiTietDonHangRepositories.GetChiTietDonHangByDonHangId(maDonHang);
            if (chiTietDonHangs == null || !chiTietDonHangs.Any())
            {
                return NotFound();
            }
            return Ok(chiTietDonHangs);
        }

    }
}
