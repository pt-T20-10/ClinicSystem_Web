using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class TonKhoTheoThang
{
    public int MaTonThang { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public string ThangNam { get; set; } = null!;

    public DateTime? NgayGhiNhan { get; set; }

    public int? SuDungDuoc { get; set; }

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;
}
