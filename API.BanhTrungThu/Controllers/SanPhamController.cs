using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Implementation;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamRepositories _sanPhamRepositories;
        private readonly IAnhSanPhamRepositories _anhSanPhamRepositories;

        public SanPhamController(ISanPhamRepositories sanPhamRepositories, IAnhSanPhamRepositories anhSanPhamRepositories)
        {
            _sanPhamRepositories = sanPhamRepositories;
            _anhSanPhamRepositories = anhSanPhamRepositories;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSanPham([FromBody] CreateSanPhamRequestDto request)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string maSanPham = "SP" + randomValue.ToString("D4");
            var sanPham = new SanPham
            {
                MaSanPham = maSanPham,
                MaLoai = request.MaLoai,
                TenSanPham = request.TenSanPham,
                Gia = request.Gia,
                MoTa = request.MoTa,
                SoLuongTrongKho = request.SoLuongTrongKho,
                NgayThem = DateTime.Now,
                TinhTrang = "Đang hoạt động"
            };

            await _sanPhamRepositories.CreateAsync(sanPham);

            var response = new SanPhamDto
            {
                MaSanPham = sanPham.MaSanPham,
                MaLoai = sanPham.MaLoai,
                TenSanPham = sanPham.TenSanPham,
                Gia = sanPham.Gia,
                MoTa = sanPham.MoTa,
                SoLuongTrongKho = sanPham.SoLuongTrongKho,
                NgayThem = sanPham.NgayThem,
                TinhTrang = sanPham.TinhTrang
            };
            if (request.ImgSelected != null)
            {
                // Tạo thư mục 'uploads' nếu nó chưa tồn tại
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                for (int i = 0; i < request.ImgSelected.Length; i++)
                {
                    // Tách chuỗi Base64 và loại media
                    var parts = request.ImgSelected[i].Split(',');
                    string mediaType = parts[0]; // Ví dụ: "data:image/jpeg;base64"
                    string base64 = parts[1];

                    // Chuyển đổi chuỗi Base64 thành mảng byte
                    byte[] imageBytes = Convert.FromBase64String(base64);

                    // Xác định định dạng file từ loại media
                    var format = mediaType.Split(';')[0].Split('/')[1]; // Ví dụ: "jpeg"

                    // Tạo tên file duy nhất cho mỗi hình ảnh
                    string fileName = $"image_{i}_{DateTime.Now.Ticks}.{format}";

                    // Tạo đường dẫn đầy đủ cho file
                    string filePath = Path.Combine(folderPath, fileName);

                    // Ghi mảng byte vào file
                    System.IO.File.WriteAllBytes(filePath, imageBytes);

                    var anhSanPham = new AnhSanPham
                    {
                        MaSanPham = maSanPham,
                        TenAnh = fileName,
                    };
                    await _anhSanPhamRepositories.UploadImg(anhSanPham);
                }

            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSanPham()
        {
            var sanPhams = await _sanPhamRepositories.GetAllAsync();

            var response = new List<SanPhamDto>();
            foreach (var sanPham in sanPhams)
            {
                var anhSanPham = await _anhSanPhamRepositories.GetAnhSanPhamById(sanPham.MaSanPham);
                response.Add(new SanPhamDto
                {
                    MaSanPham = sanPham.MaSanPham,
                    MaLoai = sanPham.MaLoai,
                    TenSanPham = sanPham.TenSanPham,
                    Gia = sanPham.Gia,
                    MoTa = sanPham.MoTa,
                    SoLuongTrongKho = sanPham.SoLuongTrongKho,
                    NgayThem = sanPham.NgayThem,
                    TinhTrang = sanPham.TinhTrang,
                    AnhSanPham = anhSanPham,
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSanPhamById(string id)
        {
            var sanPham = await _sanPhamRepositories.GetSanPhamById(id);
            var anhSanPham = await _anhSanPhamRepositories.GetAnhSanPhamById(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            var response = new SanPhamDto
            {
                MaSanPham = sanPham.MaSanPham,
                MaLoai = sanPham.MaLoai,
                TenSanPham = sanPham.TenSanPham,
                Gia = sanPham.Gia,
                MoTa = sanPham.MoTa,
                SoLuongTrongKho = sanPham.SoLuongTrongKho,
                TinhTrang = sanPham.TinhTrang,
                NgayThem = sanPham.NgayThem,
                AnhSanPham = anhSanPham,
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateSanPham(string id, UpdateSanPhamRequestDto request)
        {
            var sanPham = new SanPham
            {
                MaSanPham = id,
                MaLoai = request.MaLoai,
                TenSanPham = request.TenSanPham,
                Gia = request.Gia,
                MoTa = request.MoTa,
                SoLuongTrongKho = request.SoLuongTrongKho,
                NgayThem = request.NgayThem,
                TinhTrang = request.TinhTrang,
            };
            sanPham = await _sanPhamRepositories.UpdateAsync(sanPham);

            if (sanPham == null)
            {
                return NotFound();
            }
            //xóa ảnh đã có trong db
            if (request.AnhSanPhamDb != null)
            {
                foreach (var item in request.AnhSanPhamDb)
                {
                    _anhSanPhamRepositories.RemoveImgByName(item);
                }
            }
            //thêm ảnh mới vào db
            if (request.AnhSanPhamBrowse != null)
            {
                // Tạo thư mục 'uploads' nếu nó chưa tồn tại
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                for (int i = 0; i < request.AnhSanPhamBrowse.Length; i++)
                {
                    // Tách chuỗi Base64 và loại media
                    var parts = request.AnhSanPhamBrowse[i].Split(',');
                    string mediaType = parts[0]; // Ví dụ: "data:image/jpeg;base64"
                    string base64 = parts[1];

                    // Chuyển đổi chuỗi Base64 thành mảng byte
                    byte[] imageBytes = Convert.FromBase64String(base64);

                    // Xác định định dạng file từ loại media
                    var format = mediaType.Split(';')[0].Split('/')[1]; // Ví dụ: "jpeg"

                    // Tạo tên file duy nhất cho mỗi hình ảnh
                    string fileName = $"image_{i}_{DateTime.Now.Ticks}.{format}";

                    // Tạo đường dẫn đầy đủ cho file
                    string filePath = Path.Combine(folderPath, fileName);

                    // Ghi mảng byte vào file
                    System.IO.File.WriteAllBytes(filePath, imageBytes);

                    var anhSanPham = new AnhSanPham
                    {
                        MaSanPham = sanPham.MaSanPham,
                        TenAnh = fileName,
                    };
                    await _anhSanPhamRepositories.UploadImg(anhSanPham);
                }
            }
            var response = new SanPhamDto
            {
                MaSanPham = sanPham.MaSanPham,
                MaLoai = sanPham.MaLoai,
                TenSanPham = sanPham.TenSanPham,
                Gia = sanPham.Gia,
                MoTa = sanPham.MoTa,
                SoLuongTrongKho = sanPham.SoLuongTrongKho,
                TinhTrang = sanPham.TinhTrang,
            };
            return Ok(response);
        }

        [HttpGet("Loai/{maLoai}")]
        public async Task<ActionResult<IEnumerable<SanPham>>> GetSanPhamByLoaiAsync(string maLoai)
        {
            var sanPhams = await _sanPhamRepositories.GetSanPhamByLoaiAsync(maLoai);
            if (sanPhams == null)
            {
                return NotFound();
            }
            // Lọc chỉ lấy sản phẩm đầu tiên của mỗi loại sản phẩm
            var firstSanPhams = sanPhams.GroupBy(sp => sp.MaLoai).Select(group => group.First());
            return Ok(sanPhams);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLoaiSanPham([FromRoute] string id)
        {
            var sanPham = await _sanPhamRepositories.DeleteAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            var response = new SanPhamDto
            {
                MaSanPham = sanPham.MaSanPham,
                MaLoai = sanPham.MaLoai,
                TenSanPham = sanPham.TenSanPham,
                Gia = sanPham.Gia,
                MoTa = sanPham.MoTa,
                SoLuongTrongKho = sanPham.SoLuongTrongKho,
                TinhTrang = sanPham.TinhTrang,
                NgayThem = sanPham.NgayThem,
            };
            return Ok(response);
        }

        [HttpGet("noibat")]
        public async Task<IActionResult> GetSanPhamNoiBat()
        {
            var sanPhams = await _sanPhamRepositories.GetSanPhamNoiBatAsync();

            var response = new List<SanPhamDto>();
            foreach (var sanPham in sanPhams)
            {
                var anhSanPham = await _anhSanPhamRepositories.GetAnhSanPhamById(sanPham.MaSanPham);
                response.Add(new SanPhamDto
                {
                    MaSanPham = sanPham.MaSanPham,
                    MaLoai = sanPham.MaLoai,
                    TenSanPham = sanPham.TenSanPham,
                    Gia = sanPham.Gia,
                    MoTa = sanPham.MoTa,
                    SoLuongTrongKho = sanPham.SoLuongTrongKho,
                    NgayThem = sanPham.NgayThem,
                    TinhTrang = sanPham.TinhTrang,
                    AnhSanPham = anhSanPham,
                });
            }
            return Ok(response);
        }
    }
}
