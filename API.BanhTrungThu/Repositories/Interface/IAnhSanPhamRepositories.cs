using API.BanhTrungThu.Models.Domain;

namespace API.BanhTrungThu.Repositories.Interface
{
    public interface IAnhSanPhamRepositories
    {
        Task<IEnumerable<AnhSanPham>> GetAllAsync();
        Task<IEnumerable<AnhSanPham>> GetAnhSanPhamById(string id);
        Task<AnhSanPham> UploadImg(AnhSanPham anhSanPham);
        Task<AnhSanPham> DeleteImg(string id);
        Task<SanPham> GetSanPhamById(string idSanPham);
        string RemoveImgByName(string AnhSanPham);
    }
}
