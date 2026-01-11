using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class TonKho
{
    public int MaTonKho { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? SuDungDuoc { get; set; }

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;
}
