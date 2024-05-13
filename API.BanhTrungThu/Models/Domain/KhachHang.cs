using System.ComponentModel.DataAnnotations;

namespace API.BanhTrungThu.Models.Domain
{
    public class KhachHang
    {
        [Key]
        [StringLength(6)]
        public string MaKhachHang { get; set; }
        [StringLength(70)]
        public string TenKhachHang { get; set; }
        [StringLength(11)]
        public string SoDienThoai { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(4)]
        public string GioiTinh { get; set; }
        [StringLength(200)]
        public string DiaChi { get; set; }
    }
}
