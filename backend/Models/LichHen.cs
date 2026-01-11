using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class LichHen
{
    public int MaLichHen { get; set; }

    public int MaBenhNhan { get; set; }

    public int? MaBacSi { get; set; }

    public DateTime NgayHen { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? GhiChu { get; set; }

    public DateTime? NgayTao { get; set; }

    public int MaLoaiLichHen { get; set; }

    public bool? DaXoa { get; set; }

    public virtual NhanVien? MaBacSiNavigation { get; set; }

    public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;

    public virtual LoaiLichHen MaLoaiLichHenNavigation { get; set; } = null!;
}
