namespace API.BanhTrungThu.Models.DTO
{
    public class UpdateDonHangRequestDto
    {
        public string MaKhachHang { get; set; }
        public DateTime? ThoiGianDatHang { get; set; }
        public double TongTien { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string ThongTinThanhToan { get; set; }
        public string TinhTrang { get; set; }
    }
}
