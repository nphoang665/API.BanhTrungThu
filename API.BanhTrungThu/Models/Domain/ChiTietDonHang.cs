using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.BanhTrungThu.Models.Domain
{
    public class ChiTietDonHang
    {
        [Key]
        [StringLength(6)]
        public string MaChiTiet { get; set; }
        [ForeignKey("DonHang")]
        [StringLength(6)]
        public string MaDonHang { get; set; }
        public DonHang DonHang { get; set; }
        [ForeignKey("SanPham")]
        [StringLength(6)]
        public string MaSanPham { get; set; }
        public SanPham SanPham { get; set; }
        public int SoLuong { get; set; }
        public double Gia { get; set; }
    }
}
