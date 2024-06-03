namespace API.BanhTrungThu.Models.DTO
{
    public class CreateChiTietDonHangRequestDto
    {
        public string MaDonHang { get; set; }
        public string MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public double Gia { get; set; }
    }
}
