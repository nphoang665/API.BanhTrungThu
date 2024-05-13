using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.BanhTrungThu.Models.Domain
{
    public class SanPham
    {
        [Key]
        [StringLength(6)]
        public string MaSanPham { get; set; }
        [ForeignKey("LoaiSanPham")]
        [StringLength(6)]
        public string MaLoai { get; set; }
        public LoaiSanPham LoaiSanPham { get; set; }
        [StringLength(100)]
        public string TenSanPham { get; set; }
        public double Gia { get; set; }
        [StringLength(200)]
        public string MoTa { get; set; }
        public int SoLuongTrongKho { get; set; }
        [Column(TypeName = "Date")]
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayNhap { get; set; }
        [StringLength(50)]
        public string TinhTrang { get; set; }



    }
}
