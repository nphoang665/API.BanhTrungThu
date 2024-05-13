using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly ILoaiSanPhamRepositories _loaiSanPhamRepositories;

        public LoaiSanPhamController(ILoaiSanPhamRepositories loaiSanPhamRepositories)
        {
            _loaiSanPhamRepositories = loaiSanPhamRepositories;
        }
        [HttpPost]
        public async Task<IActionResult> CreateLoaiSanPham([FromBody] CreateLoaiSanPhamRequestDto request)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string MaLoai = "LSP" + randomValue.ToString("D3");

            var loaiSanPham = new LoaiSanPham
            {
                MaLoai = MaLoai,
                TenLoai = request.TenLoai,
                KhoiLuong = request.KhoiLuong,
            };
            await _loaiSanPhamRepositories.CreateAsync(loaiSanPham);

            var response = new LoaiSanPhamDto
            {
                MaLoai = loaiSanPham.MaLoai,
                TenLoai = loaiSanPham.TenLoai,
                KhoiLuong = loaiSanPham.KhoiLuong
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoaiSanPham()
        {
            var loaiSanPhams = await _loaiSanPhamRepositories.GetAllAsync();

            var response = new List<LoaiSanPhamDto>();
            foreach (var loaiSanPham in loaiSanPhams)
            {
                response.Add(new LoaiSanPhamDto
                {
                    MaLoai = loaiSanPham.MaLoai,
                    TenLoai = loaiSanPham.TenLoai,
                    KhoiLuong = loaiSanPham.KhoiLuong
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetLoaiSanPhamById(string id)
        {
            var existingLoaiSanPham = await _loaiSanPhamRepositories.GetLoaiSanPhamById(id);
            if (existingLoaiSanPham == null)
            {
                return NotFound();
            }
            var response = new LoaiSanPhamDto
            {
                MaLoai = existingLoaiSanPham.MaLoai,
                TenLoai = existingLoaiSanPham.TenLoai,
                KhoiLuong = existingLoaiSanPham.KhoiLuong
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateLoaiSanPham([FromRoute] string id, UpdateLoaiSanPhamRequestDto request)
        {
            var loaiSanPham = new LoaiSanPham
            {
                MaLoai = id,
                TenLoai = request.TenLoai,
                KhoiLuong = request.KhoiLuong
            };
            loaiSanPham = await _loaiSanPhamRepositories.UpdateAsync(loaiSanPham);

            if (loaiSanPham == null)
            {
                return NotFound();
            }
            var response = new LoaiSanPhamDto
            {
                MaLoai = loaiSanPham.MaLoai,
                TenLoai = loaiSanPham.TenLoai,
                KhoiLuong = loaiSanPham.KhoiLuong
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLoaiSanPham([FromRoute] string id)
        {
            var loaiSanPham = await _loaiSanPhamRepositories.DeleteAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }
            var response = new LoaiSanPhamDto
            {
                MaLoai = loaiSanPham.MaLoai,
                TenLoai = loaiSanPham.TenLoai,
                KhoiLuong = loaiSanPham.KhoiLuong
            };
            return Ok(response);
        }
    }
}
