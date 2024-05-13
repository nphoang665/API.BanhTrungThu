using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.Domain
{
    public class PhieuNhapHang
    {
        [Key]
        [StringLength(6)]
        public string MaPhieu { get; set; }
        [ForeignKey("SanPham")]
        [StringLength(6)]
        public string MaSanPham { get; set; }
        public SanPham SanPham { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayNhap { get; set; }
    }
}
