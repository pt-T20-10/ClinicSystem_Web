using System;
using System.Collections.Generic;

namespace ClinicManagement.API.Models;

public partial class DonVi
{
    public int MaDonVi { get; set; }

    public string TenDonVi { get; set; } = null!;

    public string? MoTa { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Thuoc> Thuocs { get; set; } = new List<Thuoc>();
}
