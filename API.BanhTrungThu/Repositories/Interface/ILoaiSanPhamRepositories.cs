using API.BanhTrungThu.Models.Domain;

namespace API.BanhTrungThu.Repositories.Interface
{
    public interface ILoaiSanPhamRepositories
    {
        Task<LoaiSanPham> CreateAsync(LoaiSanPham loaiSanPham);
        Task<IEnumerable<LoaiSanPham>> GetAllAsync();
        Task<LoaiSanPham?> UpdateAsync(LoaiSanPham loaiSanPham);
        Task<LoaiSanPham?> DeleteAsync(string id);
        Task<LoaiSanPham?> GetLoaiSanPhamById(string id);
        Task<LoaiSanPham> GetByIdAsync(string id);
        byte[] ExportLoaiSanPhamToExcel();
    }
}
