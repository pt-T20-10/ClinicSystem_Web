using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class DanhMucThuoc
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Thuoc> Thuocs { get; set; } = new List<Thuoc>();
}
