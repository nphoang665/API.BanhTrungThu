using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.DTO
{
    public class CreateLoaiSanPhamRequestDto
    {
        public string TenLoai { get; set; }
        public double KhoiLuong { get; set; }
    }
}
