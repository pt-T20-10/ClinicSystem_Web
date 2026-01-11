using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class ChiTietHoaDon
{
    public int MaChiTiet { get; set; }

    public int MaHoaDon { get; set; }

    public int? MaThuoc { get; set; }

    public int? MaLoThuoc { get; set; }

    public int? SoLuong { get; set; }

    public decimal? GiaBan { get; set; }

    public string? TenDichVu { get; set; }

    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;

    public virtual LoThuoc? MaLoThuocNavigation { get; set; }

    public virtual Thuoc? MaThuocNavigation { get; set; }
}
