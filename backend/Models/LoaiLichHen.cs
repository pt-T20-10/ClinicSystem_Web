using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class LoaiLichHen
{
    public int MaLoaiLichHen { get; set; }

    public string TenLoai { get; set; } = null!;

    public bool? DaXoa { get; set; }

    public string? MoTa { get; set; }

    public decimal GiaTien { get; set; }

    public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();
}
