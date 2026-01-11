using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuocController : ControllerBase
    {
        private readonly ClinicManagementContext _context;

        public ThuocController(ClinicManagementContext context)
        {
            _context = context;
        }

        // GET: api/Thuoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetThuoc([FromQuery] string? keyword)
        {
            var query = _context.Thuocs
                .Include(t => t.LoThuocs) // Kèm theo thông tin Lô thuốc
                .Include(t => t.MaDonViNavigation) // Kèm tên Đơn vị tính
                .Include(t => t.MaDanhMucNavigation) // Kèm tên Danh mục
                .Where(t => t.DaXoa != true); // Chỉ lấy thuốc chưa xóa

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.TenThuoc.Contains(keyword));
            }

            var result = await query.Select(t => new
            {
                t.MaThuoc,
                t.TenThuoc,
                TenDanhMuc = t.MaDanhMucNavigation != null ? t.MaDanhMucNavigation.TenDanhMuc : "",
                TenDonVi = t.MaDonViNavigation != null ? t.MaDonViNavigation.TenDonVi : "",
                // Logic: Lấy giá bán của Lô nhập gần nhất đang bán
                GiaBan = t.LoThuocs
                            .Where(l => l.DangBan == true && l.DaHuy == false)
                            .OrderByDescending(l => l.NgayNhap)
                            .Select(l => l.GiaBan)
                            .FirstOrDefault() ?? 0, // Nếu không có lô nào thì giá = 0
                // Logic: Tổng tồn kho = Tổng cột ConLai của các lô
                SoLuongTon = t.LoThuocs
                            .Where(l => l.DangBan == true && l.DaHuy == false)
                            .Sum(l => l.ConLai ?? 0),
                HinhAnh = "https://placehold.co/200" // Tạm thời dùng ảnh mẫu, sau này xử lý ảnh thật sau
            }).ToListAsync();

            // Chỉ trả về các thuốc có Giá > 0 (tức là đang có hàng bán)
            return Ok(result.Where(x => x.GiaBan > 0));
        }

        // GET: api/Thuoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetThuocDetail(int id)
        {
            var t = await _context.Thuocs
                .Include(t => t.LoThuocs)
                .Include(t => t.MaDonViNavigation)
                .Include(t => t.MaDanhMucNavigation)
                .FirstOrDefaultAsync(x => x.MaThuoc == id);

            if (t == null) return NotFound();

            var detail = new
            {
                t.MaThuoc,
                t.TenThuoc,
                TenDanhMuc = t.MaDanhMucNavigation?.TenDanhMuc,
                TenDonVi = t.MaDonViNavigation?.TenDonVi,
                GiaBan = t.LoThuocs
                            .Where(l => l.DangBan == true && l.DaHuy == false)
                            .OrderByDescending(l => l.NgayNhap)
                            .Select(l => l.GiaBan)
                            .FirstOrDefault() ?? 0,
                SoLuongTon = t.LoThuocs
                            .Where(l => l.DangBan == true && l.DaHuy == false)
                            .Sum(l => l.ConLai ?? 0),
                MoTa = "Công dụng: " + (t.MaQr ?? "Đang cập nhật") // Ví dụ lấy tạm trường nào đó làm mô tả
            };

            return Ok(detail);
        }
    }
}