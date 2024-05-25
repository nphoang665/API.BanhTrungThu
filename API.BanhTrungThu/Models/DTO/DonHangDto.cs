using API.BanhTrungThu.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.DTO
{
    public class DonHangDto
    {
        public string MaDonHang { get; set; }
        public string MaKhachHang { get; set; }
        public DateTime? ThoiGianDatHang { get; set; }
        public double TongTien { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string ThongTinThanhToan { get; set; }
        public string TinhTrang { get; set; }
    }
}
