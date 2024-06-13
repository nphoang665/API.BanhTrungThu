using API.BanhTrungThu.Models.Domain;

namespace API.BanhTrungThu.Repositories.Interface
{
    public interface IChiTietDonHangRepositories
    {
        Task<ChiTietDonHang> CreateAsync(ChiTietDonHang chiTietDonHang);
        Task<IEnumerable<ChiTietDonHang>> GetAllAsync();
        Task<IEnumerable<ChiTietDonHang?>> GetDonHangById(string id);
        Task<ChiTietDonHang?> UpdateAsync(ChiTietDonHang chiTietDonHang);
        Task<ChiTietDonHang?> DeleteAsync(string id);
        Task<IEnumerable<ChiTietDonHang>> GetChiTietDonHangByDonHangId(string maDonHang);
    }
}
