using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.BanhTrungThu.Models.Domain
{
    public class DonHang
    {
        [Key]
        [StringLength(6)]
        public string MaDonHang { get; set; }
        [ForeignKey("KhachHang")]
        [StringLength(6)]
        public string MaKhachHang { get; set; }
        public KhachHang KhachHang { get; set; }
        public DateTime? ThoiGianDatHang { get; set; }
        public double TongTien { get; set; }
        [StringLength(200)]
        public string DiaChiGiaoHang { get; set; }
        [StringLength(50)]
        public string ThongTinThanhToan { get; set; }

        [StringLength(50)]
        public string TinhTrang { get; set; }
    }
}
