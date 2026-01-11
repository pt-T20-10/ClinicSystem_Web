using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.API.Utils; // <--- Nhớ using Utils

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ClinicManagementContext _context;

        public AuthController(ClinicManagementContext context)
        {
            _context = context;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // 1. Kiểm tra SĐT
            var existingUser = await _context.TaiKhoanNguoiDungs
                .FirstOrDefaultAsync(u => u.SoDienThoai == request.Phone);

            if (existingUser != null)
                return BadRequest(new { message = "Số điện thoại này đã được đăng ký!" });

            // 2. Tạo hồ sơ bệnh nhân
            var newPatient = new BenhNhan
            {
                HoTen = request.FullName,
                SoDienThoai = request.Phone,
                NgayTao = DateTime.Now,
                DaXoa = false
            };

            _context.BenhNhans.Add(newPatient);
            await _context.SaveChangesAsync();

            // 3. Tạo tài khoản (CÓ HASH MẬT KHẨU)
            // ======================================================
            string passwordHash = HashUtility.ComputeSha256Hash(request.Password); 
            // ======================================================

            var newUser = new TaiKhoanNguoiDung
            {
                SoDienThoai = request.Phone,
                MatKhauHash = passwordHash, // Lưu mật khẩu đã mã hóa
                MaBenhNhan = newPatient.MaBenhNhan,
                NgayTao = DateTime.Now,
                TrangThai = true
            };

            _context.TaiKhoanNguoiDungs.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!", userId = newUser.MaTaiKhoan });
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Hash mật khẩu người dùng vừa nhập để so sánh
            // ======================================================
            string inputHash = HashUtility.ComputeSha256Hash(request.Password);
            // ======================================================

            // 2. So sánh Hash với Hash trong Database
            var user = await _context.TaiKhoanNguoiDungs
                .FirstOrDefaultAsync(u => u.SoDienThoai == request.Phone && u.MatKhauHash == inputHash);

            if (user == null)
            {
                return Unauthorized(new { message = "Sai số điện thoại hoặc mật khẩu!" });
            }
            
            // Check thêm trạng thái
            if (user.TrangThai == false)
            {
                return Unauthorized(new { message = "Tài khoản này đã bị khóa!" });
            }

            // Lấy thêm tên bệnh nhân để hiển thị
            var benhNhan = await _context.BenhNhans.FindAsync(user.MaBenhNhan);

            return Ok(new 
            { 
                message = "Đăng nhập thành công!", 
                userId = user.MaTaiKhoan, 
                maBenhNhan = user.MaBenhNhan,
                fullName = benhNhan?.HoTen ?? user.SoDienThoai,
                phone = user.SoDienThoai 
            });
        }
    }
    
    // Các class DTO giữ nguyên...
    public class RegisterRequest
    {
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
    }

    public class LoginRequest
    {
        public required string Phone { get; set; }
        public required string Password { get; set; }
    }
}