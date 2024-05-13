using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnhSanPhamController : ControllerBase
    {
        private readonly IAnhSanPhamRepositories _anhSanPhamRepositories;

        public AnhSanPhamController(IAnhSanPhamRepositories anhSanPhamRepositories)
        {
            _anhSanPhamRepositories = anhSanPhamRepositories;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAnhSanPham()
        {

            var anhSanPhams = await _anhSanPhamRepositories.GetAllAsync();

            var response = new List<AnhSanPhamDto>();
            foreach (var anhSanPham in anhSanPhams)
            {

                response.Add(new AnhSanPhamDto
                {
                    MaAnh = anhSanPham.MaAnh,
                    MaSanPham = anhSanPham.MaSanPham,
                    TenAnh = anhSanPham.TenAnh,
                    NgayThem = anhSanPham.NgayThem,
                    TenSanPham = anhSanPham.SanPham.TenSanPham
                });
            }
            return Ok(response);
        }
    }
}
