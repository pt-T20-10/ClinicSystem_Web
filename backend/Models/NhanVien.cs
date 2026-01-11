using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class NhanVien
{
    public int MaNhanVien { get; set; }

    public int? MaChuyenKhoa { get; set; }

    public string? LinkChungChi { get; set; }

    public string? LichLamViec { get; set; }

    public string? SoDienThoai { get; set; }

    public bool? DaXoa { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public int? MaVaiTro { get; set; }

    public string? HoTen { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<HoSoBenhAn> HoSoBenhAns { get; set; } = new List<HoSoBenhAn>();

    public virtual ICollection<HoaDon> HoaDonNhanVienKeDonNavigations { get; set; } = new List<HoaDon>();

    public virtual ICollection<HoaDon> HoaDonNhanVienXacNhanNavigations { get; set; } = new List<HoaDon>();

    public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();

    public virtual ICollection<LoThuoc> LoThuocs { get; set; } = new List<LoThuoc>();

    public virtual ChuyenKhoaBacSi? MaChuyenKhoaNavigation { get; set; }

    public virtual VaiTro? MaVaiTroNavigation { get; set; }

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
