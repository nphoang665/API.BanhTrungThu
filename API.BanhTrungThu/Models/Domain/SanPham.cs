using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.BanhTrungThu.Models.Domain
{
    public class SanPham
    {
        [Key]
        [StringLength(6)]
        public string MaSanPham { get; set; }
        [StringLength(6)]

        public string MaLoai { get; set; }

        [StringLength(100)]
        public string TenSanPham { get; set; }

        public double Gia { get; set; }

        public string MoTa { get; set; }

        public int SoLuongTrongKho { get; set; }

        public DateTime? NgayThem { get; set; }

        [StringLength(50)]
        public string TinhTrang { get; set; }

        [ForeignKey("MaLoai")]
        public LoaiSanPham LoaiSanPham { get; set; }

    }
}
