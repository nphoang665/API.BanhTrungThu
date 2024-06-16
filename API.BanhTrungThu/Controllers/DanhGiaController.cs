using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly IDanhGiaRepository _danhGiaRepository;
        private readonly ApplicationDbContext _db;

        public DanhGiaController(IDanhGiaRepository danhGiaRepository,ApplicationDbContext db)
        {
            _danhGiaRepository = danhGiaRepository;
            _db = db;
        }

        [HttpGet("{maSanPham}")]
        public async Task<IActionResult> GetDanhGiaBySanPham(string maSanPham)
        {
            var danhGias = await _danhGiaRepository.GetDanhGiaBySanPham(maSanPham);

            var response = new List<DanhGiaDto>();
            foreach (var danhGia in danhGias)
            {
                response.Add(new DanhGiaDto
                {
                    MaDanhGia = danhGia.MaDanhGia,
                    MaSanPham = danhGia.MaSanPham,
                    MaKhachHang = danhGia.MaKhachHang,
                    DiemDanhGia = danhGia.DiemDanhGia,
                    BinhLuan = danhGia.BinhLuan,
                    NgayDanhGia = danhGia.NgayDanhGia
                });
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddDanhGia([FromBody] DanhGiaDto request)
        {
            // Kiểm tra xem khách hàng đã mua sản phẩm này chưa
            var hasPurchased = await _db.ChiTietDonHang
                .AnyAsync(c => c.DonHang.MaKhachHang == request.MaKhachHang && c.MaSanPham == request.MaSanPham);

            if (!hasPurchased)
            {
                return BadRequest("Bạn cần mua sản phẩm này trước khi có thể đánh giá.");
            }

            // Kiểm tra xem khách hàng đã đánh giá sản phẩm này chưa
            var existingDanhGia = await _db.DanhGia
                .FirstOrDefaultAsync(dg => dg.MaSanPham == request.MaSanPham && dg.MaKhachHang == request.MaKhachHang);

            if (existingDanhGia != null)
            {
                return BadRequest("Bạn đã đánh giá sản phẩm này trước đó.");
            }

            var danhGia = new DanhGia
            {
                MaSanPham = request.MaSanPham,
                MaKhachHang = request.MaKhachHang,
                DiemDanhGia = request.DiemDanhGia,
                BinhLuan = request.BinhLuan,
                NgayDanhGia = DateTime.Now
            };
            await _danhGiaRepository.AddDanhGia(danhGia);

            return Ok(danhGia);
        }

        [HttpGet("checkMuaHang")]
        public async Task<IActionResult> checkMuaHang(string maKhachHang, string maSanPham)
        {
            var hasPurchased = await _db.ChiTietDonHang
                .AnyAsync(c => c.DonHang.MaKhachHang == maKhachHang && c.MaSanPham == maSanPham);
            return Ok(hasPurchased);
        }

        [HttpGet("checkReviewed")]
        public async Task<IActionResult> CheckReviewed(string maKhachHang, string maSanPham)
        {
            var hasReviewed = await _db.DanhGia
                .AnyAsync(dg => dg.MaSanPham == maSanPham && dg.MaKhachHang == maKhachHang);
            return Ok(hasReviewed);
        }

    }
}
