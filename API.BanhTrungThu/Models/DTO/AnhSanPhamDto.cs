using API.BanhTrungThu.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.DTO
{
    public class AnhSanPhamDto
    {
        public int MaAnh { get; set; }
        public string MaSanPham { get; set; }
        public string TenAnh { get; set; }
        public DateTime NgayThem { get; set; }
        public string TenSanPham { get; set; }
    }
}
