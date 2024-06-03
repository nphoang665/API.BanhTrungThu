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
        Task<SanPham> GetSanPhamById(string id); // Thêm phương thức này
        Task UpdateSanPhamAsync(SanPham sanPham); // Thêm phương thức này
    }
}
