using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TongQuanController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public TongQuanController(ApplicationDbContext db) {
            _db = db;
        }
        [HttpGet("DoanhThuTheoThang")]
        public IActionResult GetDoanhThuTheoThang()
        {
            var doanhThuTheoThang = (_db.DonHang
                .AsEnumerable() // Chuyển đổi sang client-side
                .GroupBy(d => new {
                    year = parseYear(d.ThoiGianDatHang),
                    month = parseMonth(d.ThoiGianDatHang)
                })
                .Select(g => new DoanhThuTheoThang
                {
                    thang = g.Key.month + "/" + g.Key.year,
                    doanhThu = g.Sum(tt => tt.TongTien)
                })
                // Chuyển về danh sách
                .ToList());

            return Ok(doanhThuTheoThang);
        }
        string parseYear(DateTime? thoiGianDatHang)
        {
            var year = thoiGianDatHang.Value.Year.ToString();
            return year;
          }

         string parseMonth(DateTime? thoiGianDatHang)
        {
            var month = thoiGianDatHang.Value.Month.ToString();
            return month;
        }

    }
}
