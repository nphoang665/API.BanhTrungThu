using API.BanhTrungThu.Data;
using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Repositories.Implementation
{
    public class AnhSanPhamRepositories : IAnhSanPhamRepositories
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        public AnhSanPhamRepositories(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _contextAccessor = contextAccessor;
        }
        public Task<AnhSanPham> DeleteImg(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AnhSanPham>> GetAllAsync()
        {
            return await _db.AnhSanPham.Include(x => x.SanPham).ToListAsync();
        }

        public async Task<IEnumerable<AnhSanPham>> GetAnhSanPhamById(string id)
        {
            return await _db.AnhSanPham.Include(s=>s.SanPham).Where(x => x.MaSanPham == id).ToListAsync();
        }

        public async Task<SanPham> GetSanPhamById(string idSanPham)
        {
            return await _db.SanPham.FindAsync(idSanPham);
        }

        public string RemoveImgByName(string AnhSanPham)
        {
            //cắt chuỗi loại bỏ phần string localhost
            string convertImgTour = AnhSanPham.Substring(30);
            //tìm kiếm ảnh trong db ANH_TOUR
            var imgTour = _db.AnhSanPham.Where(s => s.TenAnh == convertImgTour);
            //nếu ảnh tour có trong db
            if (imgTour != null)
            {
                _db.AnhSanPham.RemoveRange(imgTour);
                _db.SaveChanges();
                string s = "Xóa ảnh thành công";
                return s;

            }
            //nếu ảnh tour không có
            else
            {
                string s = "Xóa không thành công";
                return s;
            }
        }

        public async Task<AnhSanPham> UploadImg(AnhSanPham anhSanPham)
        {
            await _db.AnhSanPham.AddAsync(anhSanPham);
            await _db.SaveChangesAsync();
            return anhSanPham;
        }
    }
}
