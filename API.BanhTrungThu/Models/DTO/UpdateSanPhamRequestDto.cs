namespace API.BanhTrungThu.Models.DTO
{
    public class UpdateSanPhamRequestDto
    {
        public string MaLoai { get; set; }
        public string TenSanPham { get; set; }
        public double Gia { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTrongKho { get; set; }
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayNhap { get; set; }
        public string TinhTrang { get; set; }
        public string[] AnhSanPhamDb { get; set; }
        public string[] AnhSanPhamBrowse { get; set; }

    }
}
