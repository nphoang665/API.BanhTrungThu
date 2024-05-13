using API.BanhTrungThu.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.DTO
{
    public class SanPhamDto
    {
        public string MaSanPham { get; set; }
        public string MaLoai { get; set; }
        public string TenSanPham { get; set; }
        public double Gia { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTrongKho { get; set; }
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayNhap { get; set; }
        public string TinhTrang { get; set; }
        public IEnumerable<AnhSanPham> AnhSanPham { get; set; }
    }
}
