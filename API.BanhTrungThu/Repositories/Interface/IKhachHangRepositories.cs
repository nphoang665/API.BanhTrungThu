using API.BanhTrungThu.Models.Domain;

namespace API.BanhTrungThu.Repositories.Interface
{
    public interface IKhachHangRepositories
    {
        Task<KhachHang> CreateAsync(KhachHang khachHang);
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang?> UpdateAsync(KhachHang khachHang);
        Task<KhachHang?> DeleteAsync(string id);
        Task<KhachHang?> GetKhachHangById(string id);
    }
}
