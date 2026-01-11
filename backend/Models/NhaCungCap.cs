using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class NhaCungCap
{
    public int MaNhaCungCap { get; set; }

    public string MaSoNhaCungCap { get; set; } = null!;

    public string TenNhaCungCap { get; set; } = null!;

    public string? NguoiLienHe { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public string? MaSoThue { get; set; }

    public bool? DangHoatDong { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<LoThuoc> LoThuocs { get; set; } = new List<LoThuoc>();
}
