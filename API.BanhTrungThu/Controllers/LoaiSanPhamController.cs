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
                AnhLoai = request.AnhLoai,
            };
            await _loaiSanPhamRepositories.CreateAsync(loaiSanPham);

            if (!string.IsNullOrEmpty(request.AnhLoai))
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Tách chuỗi Base64 và loại media
                var parts = request.AnhLoai.Split(',');
                string mediaType = parts[0];
                string base64 = parts[1];

                // Chuyển đổi chuỗi Base64 thành mảng byte
                byte[] imageBytes = Convert.FromBase64String(base64);

                // Xác định định dạng file từ loại media
                var format = mediaType.Split(';')[0].Split('/')[1];

                // Tạo tên file duy nhất cho hình ảnh
                string fileName = $"{MaLoai}_{DateTime.Now.Ticks}.{format}";

                // Tạo đường dẫn đầy đủ cho file
                string filePath = Path.Combine(folderPath, fileName);

                // Ghi mảng byte vào file
                System.IO.File.WriteAllBytes(filePath, imageBytes);

                // Cập nhật tên file vào đối tượng loaiSanPham và lưu lại vào cơ sở dữ liệu
                loaiSanPham.AnhLoai = fileName;
                await _loaiSanPhamRepositories.UpdateAsync(loaiSanPham);
            }
            var response = new LoaiSanPhamDto
            {
                MaLoai = loaiSanPham.MaLoai,
                TenLoai = loaiSanPham.TenLoai,
                AnhLoai = loaiSanPham.AnhLoai
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
                    AnhLoai = loaiSanPham.AnhLoai
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
                AnhLoai = existingLoaiSanPham.AnhLoai
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateLoaiSanPham([FromRoute] string id, UpdateLoaiSanPhamRequestDto request)
        {
            // Tìm sản phẩm hiện tại trong cơ sở dữ liệu
            var existingLoaiSanPham = await _loaiSanPhamRepositories.GetByIdAsync(id);
            if (existingLoaiSanPham == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin loại sản phẩm
            existingLoaiSanPham.TenLoai = request.TenLoai;

            if (!string.IsNullOrEmpty(request.AnhLoai) && request.AnhLoai.Contains("base64"))
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Tách chuỗi Base64 và loại media
                var parts = request.AnhLoai.Split(',');
                string mediaType = parts[0];
                string base64 = parts[1];

                // Chuyển đổi chuỗi Base64 thành mảng byte
                byte[] imageBytes = Convert.FromBase64String(base64);

                // Xác định định dạng file từ loại media
                var format = mediaType.Split(';')[0].Split('/')[1];

                // Tạo tên file duy nhất cho hình ảnh
                string fileName = $"{id}_{DateTime.Now.Ticks}.{format}";

                // Tạo đường dẫn đầy đủ cho file
                string filePath = Path.Combine(folderPath, fileName);

                // Ghi mảng byte vào file
                System.IO.File.WriteAllBytes(filePath, imageBytes);

                // Cập nhật tên file vào đối tượng loaiSanPham và lưu lại vào cơ sở dữ liệu
                existingLoaiSanPham.AnhLoai = fileName;
            }
            else if (string.IsNullOrEmpty(request.AnhLoai))
            {
                // Nếu không có ảnh mới và ảnh cũ đã tồn tại, giữ nguyên ảnh cũ
                existingLoaiSanPham.AnhLoai = existingLoaiSanPham.AnhLoai;
            }

            await _loaiSanPhamRepositories.UpdateAsync(existingLoaiSanPham);

            var response = new LoaiSanPhamDto
            {
                MaLoai = existingLoaiSanPham.MaLoai,
                TenLoai = existingLoaiSanPham.TenLoai,
                AnhLoai = existingLoaiSanPham.AnhLoai
            };

            return Ok(response);
        }


        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<IActionResult> DeleteLoaiSanPham([FromRoute] string id)
        //{
        //    var loaiSanPham = await _loaiSanPhamRepositories.DeleteAsync(id);
        //    if (loaiSanPham == null)
        //    {
        //        return NotFound();
        //    }
        //    var response = new LoaiSanPhamDto
        //    {
        //        MaLoai = loaiSanPham.MaLoai,
        //        TenLoai = loaiSanPham.TenLoai,
        //        AnhLoai = loaiSanPham.AnhLoai
        //    };
        //    return Ok(response);
        //}

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLoaiSanPham([FromRoute] string id)
        {
            var loaiSanPham = await _loaiSanPhamRepositories.DeleteAsync(id);
            if (loaiSanPham == null)
            {
                return BadRequest("Loại sản phẩm này còn sản phẩm không thể xóa");
            }
            var response = new LoaiSanPhamDto
            {
                MaLoai = loaiSanPham.MaLoai,
                TenLoai = loaiSanPham.TenLoai,
                AnhLoai = loaiSanPham.AnhLoai
            };
            return Ok(response);
        }
        [HttpGet, Route("ExportLoaiSanPham")]
        public IActionResult ExportLoaiSanPham()
        {
            var fileContents = _loaiSanPhamRepositories.ExportLoaiSanPhamToExcel();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "LoaiSanPham.xlsx"
            );
        }
    }
}
