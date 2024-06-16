using API.BanhTrungThu.Models.Domain;

namespace API.BanhTrungThu.Repositories.Interface
{
    public interface IDanhGiaRepository
    {
        Task<IEnumerable<DanhGia>> GetDanhGiaBySanPham(string maSanPham);
        Task<DanhGia> AddDanhGia(DanhGia danhGia);
    }
}
