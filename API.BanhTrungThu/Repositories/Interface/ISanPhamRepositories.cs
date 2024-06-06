using API.BanhTrungThu.Models.Domain;

namespace API.BanhTrungThu.Repositories.Interface
{
    public interface ISanPhamRepositories
    {
        Task<SanPham> CreateAsync(SanPham sanPham);
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<SanPham?> UpdateAsync(SanPham sanPham);
        Task<SanPham?> DeleteAsync(string id);
        Task<SanPham?> GetSanPhamById(string id);
        Task<IEnumerable<SanPham>> GetSanPhamByLoaiAsync(string maLoai);
        Task<IEnumerable<SanPham>> GetSanPhamNoiBatAsync();
    }
}
