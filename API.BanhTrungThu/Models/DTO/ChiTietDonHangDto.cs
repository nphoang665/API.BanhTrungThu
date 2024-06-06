using API.BanhTrungThu.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.DTO
{
    public class ChiTietDonHangDto
    {
        public string MaChiTiet { get; set; }
        public string MaDonHang { get; set; }
        public string MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public double Gia { get; set; }
        public string TenSanPham { get; set; }
    }
}
