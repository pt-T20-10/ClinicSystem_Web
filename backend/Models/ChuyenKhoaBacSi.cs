using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class ChuyenKhoaBacSi
{
    public int MaChuyenKhoa { get; set; }

    public string TenChuyenKhoa { get; set; } = null!;

    public bool? DaXoa { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
