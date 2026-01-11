using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class BaiViet
{
    public int MaBaiViet { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? NoiDungTomTat { get; set; }

    public string? NoiDungChiTiet { get; set; }

    public string? HinhAnhUrl { get; set; }

    public DateTime? NgayDang { get; set; }
}
