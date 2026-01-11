using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.API.Utils;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly ClinicManagementContext _context;

        public DoctorController(ClinicManagementContext context)
        {
            _context = context;
        }

        // GET: api/Doctor
        // Lấy danh sách bác sĩ để hiển thị lên Trang chủ (Có ảnh, Chuyên khoa)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetDoctors()
{
    var doctors = await _context.NhanViens
        .Include(n => n.MaChuyenKhoaNavigation)
        .Include(n => n.MaVaiTroNavigation)
        .Where(n => n.DaXoa != true && n.MaVaiTroNavigation.TenVaiTro == "Bác sĩ")
        .ToListAsync(); // Lấy data về RAM trước để xử lý hàm TimeHelper

    var result = doctors.Select(n => new
    {
        n.MaNhanVien,
        n.HoTen,
        ChuyenKhoa = n.MaChuyenKhoaNavigation != null ? n.MaChuyenKhoaNavigation.TenChuyenKhoa : "Đa khoa",
        n.HinhAnh,
        n.SoDienThoai,
        n.LichLamViec, // Vẫn trả về chuỗi text để hiển thị
        
        // CỘT MỚI: Tính toán sẵn danh sách ngày làm việc [1, 3, 5]
        WorkingDays = TimeHelper.GetWorkingDays(n.LichLamViec) 
    });

    return Ok(result);
}
        [HttpGet("booked-slots")]
        public async Task<ActionResult<IEnumerable<TimeSpan>>> GetBookedSlots(int doctorId, DateTime date)
        {
            var bookedSlots = await _context.LichHens
                .Where(l => l.MaBacSi == doctorId 
                            && l.DaXoa != true 
                            && l.TrangThai != "Đã hủy" 
                            && l.NgayHen.Date == date.Date)
                .Select(l => l.NgayHen.TimeOfDay) // Chỉ lấy phần giờ (TimeSpan)
                .ToListAsync();

            return Ok(bookedSlots);
        }
        // GET: api/Doctor/5
        // Lấy chi tiết để hiện trong Modal đặt lịch
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetDoctorDetail(int id)
        {
            var doctor = await _context.NhanViens
                .Include(n => n.MaChuyenKhoaNavigation)
                .Where(n => n.MaNhanVien == id)
                .Select(n => new
                {
                    n.MaNhanVien,
                    n.HoTen,
                    ChuyenKhoa = n.MaChuyenKhoaNavigation != null ? n.MaChuyenKhoaNavigation.TenChuyenKhoa : "Đa khoa",
                    n.HinhAnh,
                    n.LichLamViec,
        
                })
                .FirstOrDefaultAsync();

            if (doctor == null) return NotFound();

            return Ok(doctor);
        }
    }
}