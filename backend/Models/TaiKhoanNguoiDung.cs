using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class TaiKhoanNguoiDung
{
    public int MaTaiKhoan { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public string? Email { get; set; }

    public int? MaBenhNhan { get; set; }

    public DateTime? NgayTao { get; set; }

    public bool? TrangThai { get; set; }

    public virtual BenhNhan? MaBenhNhanNavigation { get; set; }
}
