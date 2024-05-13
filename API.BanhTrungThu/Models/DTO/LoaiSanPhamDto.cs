using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.DTO
{
    public class LoaiSanPhamDto
    {
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public double KhoiLuong { get; set; }
    }
}
