using API.BanhTrungThu.Models.Domain;
using API.BanhTrungThu.Models.DTO;
using API.BanhTrungThu.Repositories.Interface;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Management;
using System.Text;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using Microsoft.EntityFrameworkCore;

namespace API.BanhTrungThu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public class RequestData
        {
            public string OptionOtp { get; set; }
            public string Email { get; set; }
            public string MatKhauMoi { get; set; }
        }
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenReponsitory;
        private readonly IDonHangRepositories _donHangRepositories;
        private readonly IChiTietDonHangRepositories _chiTietDonHangRepositories;
        private readonly ISanPhamRepositories _sanPhamRepositories;
        private readonly IKhachHangRepositories _khachHangRepositories;

        public AuthController(UserManager<IdentityUser> userManager, IKhachHangRepositories khachHangRepositories, ITokenRepository tokenReponsitory, IDonHangRepositories donHangRepositories, IChiTietDonHangRepositories chiTietDonHangRepositories, ISanPhamRepositories sanPhamRepositories)
        {
            this.userManager = userManager;
            _khachHangRepositories = khachHangRepositories;
            this.tokenReponsitory = tokenReponsitory;
            _donHangRepositories = donHangRepositories;
            _chiTietDonHangRepositories = chiTietDonHangRepositories;
            _sanPhamRepositories = sanPhamRepositories;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    var jwtToken = tokenReponsitory.CreateJwtToken(identityUser, roles.ToList());


                    // code lấy full data khách hàng
                    var khachHangs = await _khachHangRepositories.GetAllAsync();
                    var responseKhachHang = new List<KhachHangDto>();
                    foreach (var khachHang in khachHangs)
                    {
                        responseKhachHang.Add(new KhachHangDto
                        {
                            MaKhachHang = khachHang.MaKhachHang,
                            TenKhachHang = khachHang.TenKhachHang,
                            SoDienThoai = khachHang.SoDienThoai,
                            DiaChi = khachHang.DiaChi,
                            Email = khachHang.Email,
                            TinhTrang = khachHang.TinhTrang,
                            NgayDangKy = khachHang.NgayDangKy
                        });
                    }
                    //so sánh email kiểm tra xem tk này có trong khách hàng không
                    var existKhachHang = responseKhachHang.FirstOrDefault(s => s.Email == request.Email);
                    if (existKhachHang != null)
                    {
                        var response = new LoginResponseDto
                        {
                            KhachHang = existKhachHang,
                            Email = request.Email, // email đã check đúng với mật khẩu đã đúng
                            Roles = roles.ToList(),
                            Token = jwtToken
                        };
                        return Ok(response);
                    }


                }
                else
                {
                    // Mật khẩu không đúng
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
            }
            else
            {
                // Tài khoản không tồn tại
                ModelState.AddModelError("", "Tài khoản không tồn tại");
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string idKhachHang = "KH" + randomValue.ToString("D4");
            // Create IdentityUser object
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),
            };

            //Create User 
            var identityResult = await userManager.CreateAsync(user, request.Password);
            if (identityResult.Succeeded)
            {
                // Add role to User (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Khách hàng");
                if (identityResult.Succeeded)
                {
                    var khachHang = new KhachHang
                    {
                        MaKhachHang = idKhachHang,
                        TenKhachHang = request.Username,
                        SoDienThoai = "",
                        DiaChi = "",
                        Email = request.Email,
                        TinhTrang = "Đang hoạt động",
                        NgayDangKy = DateTime.Now,
                    };

                    await _khachHangRepositories.CreateAsync(khachHang);
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto requestDto)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string idKhachHang = "KH" + randomValue.ToString("D4");


            // Xác thực token từ Google


            var idToken = requestDto.IdToken;
            var setting = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new string[] { "101863175272-n460ifjdvtb6gevl0sa64md26bt0r22v.apps.googleusercontent.com" },
            };

            // Xác thực token từ Google
            var result = await GoogleJsonWebSignature.ValidateAsync(idToken, setting);
            if (result is null)
            {
                return BadRequest();
            }

            // Lấy email từ token đã xác thực
            var userEmail = result.Email;
            requestDto.Email = result.Email;
            // Lấy thông tin từ token đã xác thực
            var userName = result.Name; // Lấy tên từ token

            // Kiểm tra xem email này đã tồn tại trong CSDL của bạn hay chưa
            var identityUser = await userManager.FindByEmailAsync(userEmail);

            if (identityUser != null)
            {
                // Nếu người dùng tồn tại, đồng bộ thông tin nếu cần (cập nhật bất kỳ thông tin nào)
                // Ví dụ: identityUser.Name = result.Name;
                // Cập nhật người dùng trong CSDL
                await userManager.UpdateAsync(identityUser);
            }
            else
            {
                // Nếu người dùng không tồn tại, tạo một người dùng mới
                var newUser = new IdentityUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                };

                // Tạo người dùng
                var identityResult = await userManager.CreateAsync(newUser);
                if (identityResult.Succeeded)
                {



                    // Thêm vai trò mặc định cho người dùng (ví dụ: "Khách hàng")
                    identityResult = await userManager.AddToRoleAsync(newUser, "Khách hàng");
                    var khachHang = new KhachHang
                    {
                        MaKhachHang = idKhachHang,
                        TenKhachHang = userName,
                        SoDienThoai = "",
                        DiaChi = "",
                        Email = userEmail,
                        TinhTrang = "Đang hoạt động",
                        NgayDangKy = DateTime.Now.Date,
                    };

                    await _khachHangRepositories.CreateAsync(khachHang);
                    if (!identityResult.Succeeded)
                    {
                        // Xử lý lỗi
                        return BadRequest("Không thể gán vai trò cho người dùng.");
                    }
                }
                else
                {
                    // Xử lý lỗi
                    return BadRequest("Không thể tạo người dùng mới.");
                }
            }
            //thực hiện tìm kiếm lại người dùng trong bảng aspnetuser để get role
            var TimKiemLaiIdentityUser = await userManager.FindByEmailAsync(userEmail);
            // Tạo JWT token cho người dùng (tương tự như mã bạn đã có)
            var roles = await userManager.GetRolesAsync(TimKiemLaiIdentityUser);
            var jwtToken = tokenReponsitory.CreateJwtToken(TimKiemLaiIdentityUser, roles.ToList());

            // code lấy full data khách hàng
            var khachHangs = await _khachHangRepositories.GetAllAsync();
            var responseKhachHang = new List<KhachHangDto>();
            foreach (var khachHang in khachHangs)
            {
                responseKhachHang.Add(new KhachHangDto
                {
                    MaKhachHang = khachHang.MaKhachHang,
                    TenKhachHang = khachHang.TenKhachHang,
                    SoDienThoai = khachHang.SoDienThoai,
                    DiaChi = khachHang.DiaChi,
                    Email = khachHang.Email,
                    TinhTrang = khachHang.TinhTrang,
                    NgayDangKy = khachHang.NgayDangKy
                });
            }
            //so sánh email kiểm tra xem tk này có trong khách hàng không
            var existKhachHang = responseKhachHang.FirstOrDefault(s => s.Email == requestDto.Email);
            if (existKhachHang != null)
            {
                var response = new LoginResponseDto
                {
                    KhachHang = existKhachHang,
                    Email = requestDto.Email, // email đã check đúng với mật khẩu đã đúng
                    Roles = roles.ToList(),
                    Token = jwtToken
                };
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("quen-mat-khau{email}")]
        public IActionResult SendVerificationCode(string email)
        {
            try
            {
                // Tạo mã xác thực ngẫu nhiên
                string verificationCode = GenerateRandomCode(6);

                // Gửi mã xác thực qua email
                SendEmail(email, verificationCode);

                return Ok(new { maXacNhan = verificationCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi.", Details = ex.Message });
            }
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return new string(code);
        }

        private void SendEmail(string toEmail, string verificationCode)
        {
            string fromEmail = "hdemo1000@gmail.com";
            string appPassword = "pcrv uodi ucre xnbw";
            string subject = "Mã xác thực của bạn.Vui lòng không để lộ";
            string body = $"Mã xác thực của bạn là: {verificationCode}";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, appPassword),
                EnableSsl = true,
            };

            smtpClient.Send(mail);
        }

        [HttpPost]
        [Route("QuenMatKhau")]
        public async Task<IActionResult> QuenMatKhau([FromBody] RequestData data)
        {
            string optionOtp = data.OptionOtp;
            string email = data.Email;
            string matKhauMoi = data.MatKhauMoi;

            if (optionOtp == "guiEmail")
            {
                Random random = new Random();
                int otp = random.Next(100000, 999999);
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("hdemo1000@gmail.com", "pcrv uodi ucre xnbw")
                };

                using (var message = new MailMessage("hdemo1000@gmail.com", email)
                {
                    Subject = "Lấy lại mật khẩu website Bánh Trung Thu Ngon",
                    Body = $"Mã OTP của bạn: {otp}"
                })
                {
                    try
                    {
                        await smtp.SendMailAsync(message);
                        return Ok(new { otp });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return BadRequest(new { error = "Error100" });
                    }
                }
            }
            else if (optionOtp == "layLaiMatKhau")
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { error = "Người dùng không tồn tại." });
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, matKhauMoi);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Mật khẩu đã được đặt lại thành công." });
                }
                else
                {
                    Console.WriteLine($"Error: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    return BadRequest(new { error = "Không thể đặt lại mật khẩu." });
                }
            }
            else
            {
                return BadRequest(new { error = "Yêu cầu không hợp lệ." });
            }
        }



        [HttpGet]
        [Route("GuiEmailChoKhachHang/{id}")]
        public async Task<IActionResult> GuiMailHoaDon(string id)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string MaXacNhan;
            Random rnd = new Random();
            MaXacNhan = rnd.Next().ToString();



            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential("hdemo1000@gmail.com", "pcrv uodi ucre xnbw");

            MailMessage mail = new MailMessage();
            //mail.To.Add("phihoang01908@gmail.com");
            var donHang = await _donHangRepositories.GetDonHangById(id);
            var khachHang = await _khachHangRepositories.GetKhachHangById(donHang.MaKhachHang);
            mail.To.Add(khachHang.Email);
            mail.From = new MailAddress("hdemo1000@gmail.com");
            mail.Subject = "Thông Báo Quan Trọng Từ Bánh Trung Thu Ngon";

            string logoUrl = "https://i.imgur.com/2VUOkoU.png";

            // Đọc nội dung từ file HTML
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Template/index.html");

            string htmlFilePath = folderPath; // Đường dẫn tới file HTML của bạn
            string htmlContent = System.IO.File.ReadAllText(htmlFilePath);
            htmlContent = await XuLyHoaDonThanhToan(htmlContent, id);

            byte[] pdf;

            // Khởi tạo trình duyệt
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Đặt nội dung HTML cho trang
                    await page.SetContentAsync(htmlContent);

                    // Chuyển đổi trang thành PDF
                    pdf = await page.PdfDataAsync(new PdfOptions { Format = PaperFormat.A4 });
                }
            }

            // Tạo tệp đính kèm từ file PDF
            Attachment attachment = new Attachment(new MemoryStream(pdf), "HoaDonDienTu.pdf");
            mail.Attachments.Add(attachment);

            await smtp.SendMailAsync(mail);
            return Ok();
        }
        async Task<string> XuLyHoaDonThanhToan(string htmlContent, string idThanhToan)
        {
            // Lấy đơn hàng
            var donHang = await _donHangRepositories.GetDonHangById(idThanhToan);
            if (donHang == null)
            {
                return "";
            }

            var responseThanhToan = new DonHangDto
            {
                MaDonHang = donHang.MaDonHang,
                MaKhachHang = donHang.MaKhachHang,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien,
                ThongTinThanhToan = donHang.ThongTinThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                TinhTrang = donHang.TinhTrang
            };

            htmlContent = htmlContent.Replace("{{MaHoaDon}}", responseThanhToan.MaDonHang);
            DateTime thoiGianDatHang = DateTime.Parse(responseThanhToan.ThoiGianDatHang.ToString());
            htmlContent = htmlContent.Replace("{{NgayTaoHoaDon}}", thoiGianDatHang.ToString("dd/MM/yyyy hh:mm"));

            var khachHang = await _khachHangRepositories.GetKhachHangById(responseThanhToan.MaKhachHang);
            var responseKhachHang = new KhachHangDto
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                SoDienThoai = khachHang.SoDienThoai,
                Email = khachHang.Email,
                DiaChi = khachHang.DiaChi,
                TinhTrang = khachHang.TinhTrang,
                NgayDangKy = khachHang.NgayDangKy,
            };

            // Gán thông tin khách hàng
            htmlContent = htmlContent.Replace("{{TenKhachHang}}", responseKhachHang.TenKhachHang);
            htmlContent = htmlContent.Replace("{{SoDienThoai}}", responseKhachHang.SoDienThoai);
            htmlContent = htmlContent.Replace("{{Email}}", responseKhachHang.Email);
            htmlContent = htmlContent.Replace("{{DiaChi}}", responseKhachHang.DiaChi);
            htmlContent = htmlContent.Replace("{{TrangThai}}", responseThanhToan.TinhTrang);

            // Lấy chi tiết đơn hàng
            var chiTietDonHangs = await _chiTietDonHangRepositories.GetAllAsync();
            var responseChiTietDonHang = new List<ChiTietDonHangDto>();
            foreach (var chiTietDonHang in chiTietDonHangs)
            {
                responseChiTietDonHang.Add(new ChiTietDonHangDto
                {
                    MaChiTiet = chiTietDonHang.MaChiTiet,
                    MaDonHang = chiTietDonHang.MaDonHang,
                    MaSanPham = chiTietDonHang.MaSanPham,
                    SoLuong = chiTietDonHang.SoLuong,
                    Gia = chiTietDonHang.Gia,
                });
            }

            if (responseChiTietDonHang.Count > 0)
            {
                var responseChiTietDonHangDuocDat = responseChiTietDonHang.Where(s => s.MaDonHang == responseThanhToan.MaDonHang).ToList();

                int startTrIndex = htmlContent.IndexOf("<tr class=\"tr_item_SanPham\">");
                if (startTrIndex != -1)
                {
                    int endTrIndex = htmlContent.IndexOf("</tr>", startTrIndex) + 5; // +5 để bao gồm cả thẻ </tr>
                    string trContent = htmlContent.Substring(startTrIndex, endTrIndex - startTrIndex);

                    // Biến để lưu nội dung HTML cuối cùng
                    StringBuilder finalHtmlContent = new StringBuilder();

                    int stt = 1;
                    // Lặp qua từng mục trong danh sách
                    foreach (var item in responseChiTietDonHangDuocDat)
                    {
                        // Sao chép nội dung HTML gốc
                        string itemHtmlContent = string.Copy(trContent);
                        var sanPham = await _sanPhamRepositories.GetSanPhamById(item.MaSanPham);

                        // Thay thế các placeholder trong HTML với dữ liệu thực tế
                        itemHtmlContent = itemHtmlContent.Replace("{{STT}}", stt.ToString());
                        itemHtmlContent = itemHtmlContent.Replace("{{TenSanPham}}", sanPham.TenSanPham);
                        itemHtmlContent = itemHtmlContent.Replace("{{SoLuong}}", item.SoLuong.ToString());
                        itemHtmlContent = itemHtmlContent.Replace("{{DonGia}}", item.Gia.ToString("N0"));
                        itemHtmlContent = itemHtmlContent.Replace("{{ThanhTien}}", (item.SoLuong * item.Gia).ToString("N0"));

                        finalHtmlContent.Append(itemHtmlContent);
                        stt++;
                    }

                    htmlContent = htmlContent.Replace(trContent, finalHtmlContent.ToString());
                }

                htmlContent = htmlContent.Replace("{{TongTien}}", responseThanhToan.TongTien.ToString("N0"));
            }

            return htmlContent;
        }


    }
}