using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangRepositories _donHangRepositories;
        private readonly IConfiguration _configuration;

        public DonHangController(IDonHangRepositories donHangRepositories, IConfiguration configuration)
        {
            _donHangRepositories = donHangRepositories;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDonHang([FromBody] CreateDonHangRequestDto request)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string maDonHang = "DH" + randomValue.ToString("D4");

            var donHang = new DonHang
            {
                MaDonHang = maDonHang,
                MaKhachHang = request.MaKhachHang,
                ThoiGianDatHang = request.ThoiGianDatHang,
                TongTien = request.TongTien,
                ThongTinThanhToan = request.ThongTinThanhToan,
                DiaChiGiaoHang = request.DiaChiGiaoHang,
                TinhTrang = "Đang xử lý"
            };
            await _donHangRepositories.CreateAsync(donHang);

            var paymentUrl = CreateVnPayPaymentUrl(donHang.TongTien.ToString(), maDonHang);

            var response = new DonHangDto
            {
                MaDonHang = donHang.MaDonHang,
                MaKhachHang = donHang.MaKhachHang,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien,
                ThongTinThanhToan = donHang.ThongTinThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                TinhTrang = donHang.TinhTrang,
                PaymentUrl = paymentUrl
            };
            return Ok(response);
        }

        private string CreateVnPayPaymentUrl(string amount, string orderInfo)
        {
            var vnp_Url = _configuration["Vnpay:Url"];
            var vnp_Returnurl = _configuration["Vnpay:ReturnUrl"];
            var vnp_TmnCode = _configuration["Vnpay:TmnCode"];
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (Convert.ToInt32(amount) * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", orderInfo);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", orderInfo);

            return vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        }

        [HttpGet("vnpay-payment-url")]
        public IActionResult GetVnPayPaymentUrl(string orderId, string amount)
        {
            var paymentUrl = CreateVnPayPaymentUrl(amount, orderId);
            return Ok(new { paymentUrl });
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];
            var vnpayData = HttpContext.Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();

            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            string inputHash = vnpayData["vnp_SecureHash"];
            if (vnpay.ValidateSignature(inputHash, vnp_HashSecret))
            {
                string txnRef = vnpayData["vnp_TxnRef"];
                string responseCode = vnpayData["vnp_ResponseCode"];

                var donHang = await _donHangRepositories.GetDonHangById(txnRef);
                if (donHang == null)
                {
                    return NotFound();
                }

                if (responseCode == "00")
                {
                    donHang.TinhTrang = "Đã thanh toán";
                    await _donHangRepositories.UpdateAsync(donHang);
                    // Chuyển hướng người dùng về trang chủ
                    return Redirect("http://localhost:4200/home");
                }
                else
                {
                    donHang.TinhTrang = "Thanh toán thất bại";
                    await _donHangRepositories.UpdateAsync(donHang);
                    // Chuyển hướng người dùng về trang thông báo lỗi
                    return Redirect("/payment-error");
                }
            }
            return BadRequest("Invalid signature");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDonHang()
        {
            var donHangs = await _donHangRepositories.GetAllAsync();

            var response = new List<DonHangDto>();
            foreach (var donHang in donHangs)
            {
                response.Add(new DonHangDto
                {
                    MaDonHang = donHang.MaDonHang,
                    MaKhachHang = donHang.MaKhachHang,
                    ThoiGianDatHang = donHang.ThoiGianDatHang,
                    TongTien = donHang.TongTien,
                    ThongTinThanhToan = donHang.ThongTinThanhToan,
                    DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                    TinhTrang = donHang.TinhTrang
                });
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonHangById(string id)
        {
            var donHang = await _donHangRepositories.GetDonHangById(id);
            if (donHang == null)
            {
                return NotFound();
            }
            var response = new DonHangDto
            {
                MaDonHang = donHang.MaDonHang,
                MaKhachHang = donHang.MaKhachHang,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien,
                ThongTinThanhToan = donHang.ThongTinThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                TinhTrang = donHang.TinhTrang
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonHang(string id, UpdateDonHangRequestDto request)
        {
            var donHang = new DonHang
            {
                MaDonHang = id,
                MaKhachHang = request.MaKhachHang,
                ThoiGianDatHang = request.ThoiGianDatHang,
                TongTien = request.TongTien,
                ThongTinThanhToan = request.ThongTinThanhToan,
                DiaChiGiaoHang = request.DiaChiGiaoHang,
                TinhTrang = request.TinhTrang
            };
            if (donHang == null)
            {
                return NotFound();
            }
            donHang = await _donHangRepositories.UpdateAsync(donHang);
            var response = new DonHangDto
            {
                MaDonHang = donHang.MaDonHang,
                MaKhachHang = donHang.MaKhachHang,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien,
                ThongTinThanhToan = donHang.ThongTinThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                TinhTrang = donHang.TinhTrang
            };
            return Ok(response);
        }
        [HttpGet]
        [Route("lich-su-mua-hang/{maKhachHang}")]
        public async Task<IActionResult> GetLichSuMuaHang(string maKhachHang)
        {
            var donHangs = await _donHangRepositories.GetLichSuMuaHangByKhachHang(maKhachHang);

            var response = new List<DonHangDto>();
            foreach (var donHang in donHangs)
            {
                var chiTietDonHangs = await _donHangRepositories.GetChiTietDonHangByMaDonHang(donHang.MaDonHang);
                response.Add(new DonHangDto
                {
                    MaDonHang = donHang.MaDonHang,
                    MaKhachHang = donHang.MaKhachHang,
                    ThoiGianDatHang = donHang.ThoiGianDatHang,
                    TongTien = donHang.TongTien,
                    ThongTinThanhToan = donHang.ThongTinThanhToan,
                    DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                    TinhTrang = donHang.TinhTrang,
                    ChiTietDonHang = chiTietDonHangs.Select(chiTiet => new ChiTietDonHangDto
                    {
                        MaChiTiet = chiTiet.MaChiTiet,
                        MaDonHang = chiTiet.MaDonHang,
                        MaSanPham = chiTiet.MaSanPham,
                        SoLuong = chiTiet.SoLuong,
                        Gia = chiTiet.Gia,
                        TenSanPham = chiTiet.SanPham.TenSanPham // Thêm dòng này
                    }).ToList()
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("don-hang-theo-khach-hang/{id}")]
        public async Task<IActionResult> GetDonHangByKhachHang(string id)
        {
            var donHangs = await _donHangRepositories.GetDonHangByKhachHang(id);
            var response = new List<DonHangDto>();
            foreach (var donHang in donHangs)
            {
                var chiTietDonHangs = await _donHangRepositories.GetChiTietDonHangByMaDonHang(donHang.MaDonHang);
                response.Add(new DonHangDto
                {
                    MaDonHang = donHang.MaDonHang,
                    MaKhachHang = donHang.MaKhachHang,
                    ThoiGianDatHang = donHang.ThoiGianDatHang,
                    TongTien = donHang.TongTien,
                    ThongTinThanhToan = donHang.ThongTinThanhToan,
                    DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                    TinhTrang = donHang.TinhTrang,
                    ChiTietDonHang = chiTietDonHangs.Select(chiTiet => new ChiTietDonHangDto
                    {
                        MaChiTiet = chiTiet.MaChiTiet,
                        MaDonHang = chiTiet.MaDonHang,
                        MaSanPham = chiTiet.MaSanPham,
                        SoLuong = chiTiet.SoLuong,
                        Gia = chiTiet.Gia,
                        TenSanPham = chiTiet.SanPham.TenSanPham
                    }).ToList()
                });
            }
            return Ok(response);
        }
    }
}

public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new SortedList<string, string>();
    private readonly SortedList<string, string> _responseData = new SortedList<string, string>();

    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _requestData.Add(key, value);
        }
    }

    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _responseData.Add(key, value);
        }
    }

    public string CreateRequestUrl(string baseUrl, string hashSecret)
    {
        var data = new StringBuilder();
        foreach (var kv in _requestData)
        {
            if (data.Length > 0)
            {
                data.Append("&");
            }
            data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
        }

        var rawData = data.ToString();
        var signData = HmacSHA512(hashSecret, rawData);
        var paymentUrl = $"{baseUrl}?{rawData}&vnp_SecureHash={signData}";
        return paymentUrl;
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        var data = new StringBuilder();
        foreach (var kv in _responseData)
        {
            if (kv.Key != "vnp_SecureHash")
            {
                if (data.Length > 0)
                {
                    data.Append("&");
                }
                data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
            }
        }

        var rawData = data.ToString();
        var myChecksum = HmacSHA512(secretKey, rawData);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private static string HmacSHA512(string key, string inputData)
    {
        var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
