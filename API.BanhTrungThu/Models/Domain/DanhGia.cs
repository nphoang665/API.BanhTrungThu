using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.Domain
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        [ForeignKey("SanPham")]
        [StringLength(6)]
        public string MaSanPham { get; set; }
        [StringLength(6)]

        [ForeignKey("KhachHang")]
        public string MaKhachHang { get; set; }

        [Range(1, 5)]
        public int DiemDanhGia { get; set; }

        public string BinhLuan { get; set; }

        public DateTime NgayDanhGia { get; set; }

        public SanPham SanPham { get; set; }

        public KhachHang KhachHang { get; set; }
    }
}
