namespace API.BanhTrungThu.Models.DTO
{
    public class CreateKhachHangRequestDto
    {
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string TinhTrang { get; set; }
    }
}
