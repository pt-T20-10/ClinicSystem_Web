using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class Thuoc
{
    public int MaThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public int? MaDanhMuc { get; set; }

    public int? MaDonVi { get; set; }

    public bool? DaXoa { get; set; }

    public string? MaVach { get; set; }

    public string? MaQr { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<LoThuoc> LoThuocs { get; set; } = new List<LoThuoc>();

    public virtual DanhMucThuoc? MaDanhMucNavigation { get; set; }

    public virtual DonVi? MaDonViNavigation { get; set; }

    public virtual ICollection<TonKhoTheoThang> TonKhoTheoThangs { get; set; } = new List<TonKhoTheoThang>();

    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
}
