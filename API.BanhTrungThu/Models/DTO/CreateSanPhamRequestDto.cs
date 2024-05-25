using API.BanhTrungThu.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.DTO
{
    public class CreateSanPhamRequestDto
    {
        public string MaLoai { get; set; }
        public string TenSanPham { get; set; }
        public double Gia { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTrongKho { get; set; }
        public DateTime NgayThem { get; set; }
        public string TinhTrang { get; set; }
        public string[] ImgSelected { get; set; }
    }
}
