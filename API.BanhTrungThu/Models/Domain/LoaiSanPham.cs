using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.BanhTrungThu.Models.Domain
{
    public class LoaiSanPham
    {
        [Key]
        [StringLength(6)]
        public string MaLoai { get; set; }
        [StringLength(70)]
        public string TenLoai { get; set; }
        public string AnhLoai { get; set; }
    }
}
