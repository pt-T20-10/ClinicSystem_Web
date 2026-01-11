using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class TaiKhoan
{
    public string TenDangNhap { get; set; } = null!;

    public int? MaBacSi { get; set; }

    public string MatKhau { get; set; } = null!;

    public string? VaiTro { get; set; }

    public bool? DaDangNhap { get; set; }

    public bool? DaXoa { get; set; }

    public virtual NhanVien? MaBacSiNavigation { get; set; }
}
