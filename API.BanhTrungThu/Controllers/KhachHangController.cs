using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangRepositories _khachHangRepositories;

        public KhachHangController(IKhachHangRepositories khachHangRepositories)
        {
            _khachHangRepositories = khachHangRepositories;
        }
        [HttpPost]
        public async Task<IActionResult> CreateKhachHang([FromBody] CreateKhachHangRequestDto request)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string maKhachHang = "KH" + randomValue.ToString("D4");

            var khachHang = new KhachHang
            {
                MaKhachHang = maKhachHang,
                TenKhachHang = request.TenKhachHang,
                SoDienThoai = request.SoDienThoai,
                Email = request.Email,
                DiaChi = request.DiaChi,
                TinhTrang = "Đang hoạt động",
                NgayDangKy = DateTime.Now,
            };

            await _khachHangRepositories.CreateAsync(khachHang);

            var response = new KhachHangDto
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                SoDienThoai = khachHang.SoDienThoai,
                Email = khachHang.Email,
                DiaChi = khachHang.DiaChi,
                TinhTrang = khachHang.TinhTrang,
                NgayDangKy = khachHang.NgayDangKy,
            };
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllKhachHang()
        {
            var khachHangs = await _khachHangRepositories.GetAllAsync();

            var response = new List<KhachHangDto>();
            foreach (var khachHang in khachHangs)
            {
                response.Add(new KhachHangDto
                {
                    MaKhachHang = khachHang.MaKhachHang,
                    TenKhachHang = khachHang.TenKhachHang,
                    SoDienThoai = khachHang.SoDienThoai,
                    Email = khachHang.Email,
                    DiaChi = khachHang.DiaChi,
                    TinhTrang = khachHang.TinhTrang,
                    NgayDangKy = khachHang.NgayDangKy,
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetKhachHangById(string id)
        {
            var khachHang = await _khachHangRepositories.GetKhachHangById(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            var response = new KhachHangDto
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                SoDienThoai = khachHang.SoDienThoai,
                Email = khachHang.Email,
                DiaChi = khachHang.DiaChi,
                TinhTrang = khachHang.TinhTrang,
                NgayDangKy = khachHang.NgayDangKy,
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateKhachHang(string id, UpdateKhachHangRequestDto request)
        {
            var khachHang = new KhachHang
            {
                MaKhachHang = id,
                TenKhachHang = request.TenKhachHang,
                SoDienThoai = request.SoDienThoai,
                Email = request.Email,
                DiaChi = request.DiaChi,
                TinhTrang = request.TinhTrang,
                NgayDangKy = request.NgayDangKy,
            };
            if(khachHang == null)
            {
                return NotFound();
            }
            khachHang = await _khachHangRepositories.UpdateAsync(khachHang);
            var response = new KhachHangDto
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                SoDienThoai = khachHang.SoDienThoai,
                Email = khachHang.Email,
                DiaChi = khachHang.DiaChi,
                TinhTrang = khachHang.TinhTrang,
                NgayDangKy = khachHang.NgayDangKy,
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteKhachHang([FromRoute] string id)
        {
            var khachHang = await _khachHangRepositories.DeleteAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            var response = new KhachHangDto
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                SoDienThoai = khachHang.SoDienThoai,
                Email = khachHang.Email,
                DiaChi = khachHang.DiaChi,
                TinhTrang = khachHang.TinhTrang,
                NgayDangKy = khachHang.NgayDangKy,
            };
            return Ok(response);
        }

        [HttpGet, Route("ExportKhachHang")]
        public IActionResult ExportKhachHang()
        {
            var fileContents = _khachHangRepositories.ExportKhachHangToExcel();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "KhachHang.xlsx"
            );
        }
    }
}
