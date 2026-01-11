using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class LoThuoc
{
    public int MaLoThuoc { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public DateTime? NgayNhap { get; set; }

    public decimal DonGia { get; set; }

    public decimal? GiaBan { get; set; }

    public decimal? ThanhTien { get; set; }

    public decimal? TiLeLoiNhuan { get; set; }

    public DateOnly? HanSuDung { get; set; }

    public int? MaNhaCungCap { get; set; }

    public int? NguoiNhap { get; set; }

    public int? ConLai { get; set; }

    public bool? DangBan { get; set; }

    public bool DaHuy { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;

    public virtual NhanVien? NguoiNhapNavigation { get; set; }
}
