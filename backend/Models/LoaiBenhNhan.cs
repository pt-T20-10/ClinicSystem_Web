using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class LoaiBenhNhan
{
    public int MaLoaiBenhNhan { get; set; }

    public string TenLoai { get; set; } = null!;

    public decimal? GiamGia { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<BenhNhan> BenhNhans { get; set; } = new List<BenhNhan>();
}
