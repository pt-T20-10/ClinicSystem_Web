using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class BenhNhan
{
    public int MaBenhNhan { get; set; }

    public string? MaSoBaoHiem { get; set; }

    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public int? MaLoaiBenhNhan { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? Email { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<HoSoBenhAn> HoSoBenhAns { get; set; } = new List<HoSoBenhAn>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();

    public virtual LoaiBenhNhan? MaLoaiBenhNhanNavigation { get; set; }

    public virtual ICollection<TaiKhoanNguoiDung> TaiKhoanNguoiDungs { get; set; } = new List<TaiKhoanNguoiDung>();
}
