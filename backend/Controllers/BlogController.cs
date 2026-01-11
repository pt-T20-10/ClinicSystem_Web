using Microsoft.AspNetCore.Mvc;
using ClinicManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ClinicManagementContext _context;

        public BlogController(ClinicManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BaiViet>>> GetBlogs()
        {
            return await _context.BaiViets.OrderByDescending(b => b.NgayDang).ToListAsync();
        }
    }
}