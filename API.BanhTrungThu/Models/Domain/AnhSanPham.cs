using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.BanhTrungThu.Models.Domain
{
    public class AnhSanPham
    {
        [Key]
        public int MaAnh { get; set; }
        [ForeignKey("SanPham")]
        [StringLength(6)]
        public string MaSanPham { get; set; }
        public SanPham SanPham { get; set; }
        [StringLength(100)]
        public string TenAnh { get; set; }
    }
}
