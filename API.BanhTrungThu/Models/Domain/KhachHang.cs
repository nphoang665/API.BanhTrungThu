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

        [Required]
        [StringLength(80)]
        public string Email { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        [StringLength(50)]
        public string TinhTrang { get; set; }
 
    }
}
