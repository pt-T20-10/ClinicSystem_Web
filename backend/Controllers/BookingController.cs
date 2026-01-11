using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.API.Utils;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ClinicManagementContext _context;

        public BookingController(ClinicManagementContext context)
        {
            _context = context;
        }

        // GET: api/Booking/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAppointmentTypes()
        {
            var types = await _context.LoaiLichHens
                .Where(x => x.DaXoa != true)
                .Select(x => new { x.MaLoaiLichHen, x.TenLoai, x.GiaTien })
                .ToListAsync();
            return Ok(types);
        }

        // POST: api/Booking (ĐẶT LỊCH MỚI)
        [HttpPost]
        public async Task<ActionResult> DatLichKham([FromBody] BookingRequest request)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(request.MaBenhNhan);
            if (benhNhan == null) return BadRequest(new { message = "Không tìm thấy thông tin bệnh nhân!" });

            if (request.MaBacSi.HasValue)
            {
                var bacSi = await _context.NhanViens.FindAsync(request.MaBacSi.Value);
                if (bacSi == null) return BadRequest(new { message = "Bác sĩ không tồn tại!" });

                // Check 1: Lịch bác sĩ làm việc
                if (!string.IsNullOrEmpty(bacSi.LichLamViec)) 
                {
                    if (!TimeHelper.IsDoctorWorkingAt(bacSi.LichLamViec, request.NgayHen))
                        return BadRequest(new { message = $"Bác sĩ không làm việc vào giờ này! Lịch: {bacSi.LichLamViec}" });
                }

                // Check 2: Trùng lịch (excludeAppointmentId = null)
                bool isAvailable = await TimeHelper.IsSlotAvailable(_context, request.MaBacSi.Value, request.NgayHen);
                if (!isAvailable)
                    return BadRequest(new { message = "Khung giờ này đã kín lịch. Vui lòng chọn giờ khác." });
            }

            try
            {
                var lichHen = new LichHen
                {
                    MaBenhNhan = request.MaBenhNhan,
                    MaBacSi = request.MaBacSi,
                    MaLoaiLichHen = request.MaLoaiLichHen,
                    NgayHen = request.NgayHen,
                    GhiChu = request.GhiChu, // <--- ĐÃ SỬA THÀNH GHI CHÚ
                    TrangThai = "Đang chờ",
                    NgayTao = DateTime.Now,
                    DaXoa = false
                };

                _context.LichHens.Add(lichHen);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Đặt lịch thành công!", maLichHen = lichHen.MaLichHen });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // PUT: api/Booking/cancel/5 (HỦY LỊCH)
        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> HuyLichHen(int id)
        {
            var lichHen = await _context.LichHens.FindAsync(id);
            if (lichHen == null) return NotFound(new { message = "Không tìm thấy lịch hẹn." });

            if (lichHen.TrangThai != "Đang chờ")
            {
                return BadRequest(new { message = "Không thể hủy lịch hẹn đã khám hoặc đang xử lý." });
            }

            lichHen.TrangThai = "Đã hủy";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã hủy lịch hẹn thành công." });
        }

        // PUT: api/Booking/reschedule/5 (DỜI LỊCH / SỬA LỊCH)
        [HttpPut("reschedule/{id}")]
        public async Task<ActionResult> DoiLichHen(int id, [FromBody] RescheduleRequest request)
        {
            var lichHen = await _context.LichHens.FindAsync(id);
            if (lichHen == null) return NotFound(new { message = "Không tìm thấy lịch hẹn." });

            if (lichHen.TrangThai != "Đang chờ")
                return BadRequest(new { message = "Chỉ có thể thay đổi lịch hẹn khi đang ở trạng thái Chờ." });

            // Check logic giờ mới
            if (lichHen.MaBacSi.HasValue)
            {
                var bacSi = await _context.NhanViens.FindAsync(lichHen.MaBacSi.Value);
                
                if (bacSi != null && !string.IsNullOrEmpty(bacSi.LichLamViec))
                {
                    if (!TimeHelper.IsDoctorWorkingAt(bacSi.LichLamViec, request.NewDate))
                        return BadRequest(new { message = $"Bác sĩ không làm việc vào giờ mới này!" });
                }

                bool isAvailable = await TimeHelper.IsSlotAvailable(_context, lichHen.MaBacSi.Value, request.NewDate, id);
                if (!isAvailable)
                    return BadRequest(new { message = "Giờ mới bạn chọn đã bị trùng với bệnh nhân khác." });
            }

            // Cập nhật thông tin
            lichHen.NgayHen = request.NewDate;
            if (!string.IsNullOrEmpty(request.GhiChu)) lichHen.GhiChu = request.GhiChu; // <--- ĐÃ SỬA
            
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật lịch hẹn thành công!" });
        }
    }

    // DTO CẬP NHẬT
    public class BookingRequest
    {
        public int MaBenhNhan { get; set; }
        public int? MaBacSi { get; set; }
        public int MaLoaiLichHen { get; set; }
        public DateTime NgayHen { get; set; }
        public string? GhiChu { get; set; } // <--- Đổi tên biến DTO luôn cho đồng bộ
    }

    public class RescheduleRequest
    {
        public DateTime NewDate { get; set; }
        public string? GhiChu { get; set; } // <--- Đổi tên biến DTO luôn
    }
}