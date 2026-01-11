using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class HoaDon
{
    public int MaHoaDon { get; set; }

    public int? MaBenhNhan { get; set; }

    public decimal TongTien { get; set; }

    public DateTime? NgayLapHoaDon { get; set; }

    public string? TrangThai { get; set; }

    public string LoaiHoaDon { get; set; } = null!;

    public int? MaHoSoBenhAn { get; set; }

    public string? GhiChu { get; set; }

    public decimal? GiamGia { get; set; }

    public decimal? Thue { get; set; }

    public int? NhanVienKeDon { get; set; }

    public int? NhanVienXacNhan { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual BenhNhan? MaBenhNhanNavigation { get; set; }

    public virtual HoSoBenhAn? MaHoSoBenhAnNavigation { get; set; }

    public virtual NhanVien? NhanVienKeDonNavigation { get; set; }

    public virtual NhanVien? NhanVienXacNhanNavigation { get; set; }
}
