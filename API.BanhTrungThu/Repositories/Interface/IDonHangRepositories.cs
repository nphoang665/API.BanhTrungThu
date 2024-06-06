using API.BanhTrungThu.Models.Domain;

namespace API.BanhTrungThu.Repositories.Interface
{
    public interface IDonHangRepositories
    {
        Task<DonHang> CreateAsync(DonHang donHang);
        Task<IEnumerable<DonHang>> GetAllAsync();
        Task<DonHang?> GetDonHangById(string id);
        Task<DonHang?> UpdateAsync(DonHang donHang);
        Task<DonHang?> DeleteAsync(string id);
        Task<SanPham> GetSanPhamById(string id);
        Task UpdateSanPhamAsync(SanPham sanPham); 
        Task<IEnumerable<DonHang>> GetDonHangByKhachHang(string maKhachHang);
        Task<IEnumerable<DonHang>> GetLichSuMuaHangByKhachHang(string maKhachHang); // Thêm dòng này
        Task<IEnumerable<ChiTietDonHang>> GetChiTietDonHangByMaDonHang(string maDonHang); // Thêm dòng này
    }
}
