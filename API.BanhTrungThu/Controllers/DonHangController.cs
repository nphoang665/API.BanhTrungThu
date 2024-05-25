using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangRepositories _donHangRepositories;

        public DonHangController(IDonHangRepositories donHangRepositories)
        {
            _donHangRepositories = donHangRepositories;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDonHang([FromBody] CreateDonHangRequestDto request)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string maDonHang = "DH" + randomValue.ToString("D4");

            var donHang = new DonHang
            {
                MaDonHang = maDonHang,
                MaKhachHang = request.MaKhachHang,
                ThoiGianDatHang = request.ThoiGianDatHang,
                TongTien = request.TongTien,
                ThongTinThanhToan = request.ThongTinThanhToan,
                DiaChiGiaoHang = request.DiaChiGiaoHang,
                TinhTrang = "Đang xử lý"
            };
            await _donHangRepositories.CreateAsync(donHang);

            var response = new DonHangDto
            {
                MaDonHang = donHang.MaDonHang,
                MaKhachHang = donHang.MaKhachHang,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien,
                ThongTinThanhToan = donHang.ThongTinThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                TinhTrang = donHang.TinhTrang
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDonHang()
        {
            var donHangs = await _donHangRepositories.GetAllAsync();

            var response = new List<DonHangDto>();
            foreach (var donHang in donHangs)
            {
                response.Add(new DonHangDto
                {
                    MaDonHang = donHang.MaDonHang,
                    MaKhachHang = donHang.MaKhachHang,
                    ThoiGianDatHang = donHang.ThoiGianDatHang,
                    TongTien = donHang.TongTien,
                    ThongTinThanhToan = donHang.ThongTinThanhToan,
                    DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                    TinhTrang = donHang.TinhTrang
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDonHangById(string id)
        {
            var donHang = await _donHangRepositories.GetDonHangById(id);
            if (donHang == null)
            {
                return NotFound();
            }
            var response = new DonHangDto
            {
                MaDonHang = donHang.MaDonHang,
                MaKhachHang = donHang.MaKhachHang,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien,
                ThongTinThanhToan = donHang.ThongTinThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                TinhTrang = donHang.TinhTrang
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateDonHang(string id,UpdateDonHangRequestDto request)
        {
            var donHang = new DonHang
            {
                MaDonHang = id,
                MaKhachHang = request.MaKhachHang,
                ThoiGianDatHang = request.ThoiGianDatHang,
                TongTien = request.TongTien,
                ThongTinThanhToan = request.ThongTinThanhToan,
                DiaChiGiaoHang = request.DiaChiGiaoHang,
                TinhTrang = request.TinhTrang
            };
            if(donHang == null )
            {
                return NotFound();
            }
            donHang = await _donHangRepositories.UpdateAsync(donHang);
            var response = new DonHangDto
            {
                MaDonHang = donHang.MaDonHang,
                MaKhachHang = donHang.MaKhachHang,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien,
                ThongTinThanhToan = donHang.ThongTinThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                TinhTrang = donHang.TinhTrang
            };
            return Ok(response);
        }
    }
}
