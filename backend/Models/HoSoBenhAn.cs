using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class HoSoBenhAn
{
    public int MaHoSo { get; set; }

    public int MaBenhNhan { get; set; }

    public int MaBacSi { get; set; }

    public string? ChanDoan { get; set; }

    public string? DonThuoc { get; set; }

    public string? KetQuaXetNghiem { get; set; }

    public string? LoiKhuyenBacSi { get; set; }

    public DateTime? NgayTao { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual NhanVien MaBacSiNavigation { get; set; } = null!;

    public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;
}
