using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.API.Models;

public partial class ClinicManagementContext : DbContext
{
    public ClinicManagementContext()
    {
    }

    public ClinicManagementContext(DbContextOptions<ClinicManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiViet> BaiViets { get; set; }

    public virtual DbSet<BenhNhan> BenhNhans { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChuyenKhoaBacSi> ChuyenKhoaBacSis { get; set; }

    public virtual DbSet<DanhMucThuoc> DanhMucThuocs { get; set; }

    public virtual DbSet<DonVi> DonVis { get; set; }

    public virtual DbSet<HoSoBenhAn> HoSoBenhAns { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<LichHen> LichHens { get; set; }

    public virtual DbSet<LoThuoc> LoThuocs { get; set; }

    public virtual DbSet<LoaiBenhNhan> LoaiBenhNhans { get; set; }

    public virtual DbSet<LoaiLichHen> LoaiLichHens { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TaiKhoanNguoiDung> TaiKhoanNguoiDungs { get; set; }

    public virtual DbSet<Thuoc> Thuocs { get; set; }

    public virtual DbSet<TonKho> TonKhos { get; set; }

    public virtual DbSet<TonKhoTheoThang> TonKhoTheoThangs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=database-clinicproject.c12wwy6e6swn.ap-southeast-1.rds.amazonaws.com;Initial Catalog=ClinicManagement;Persist Security Info=True;User ID=admin;Password=VH9jPoZrN0skyNbc;Trust Server Certificate=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.MaBaiViet).HasName("PK__BaiViet__AEDD564759B589B9");

            entity.ToTable("BaiViet");

            entity.Property(e => e.HinhAnhUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.NgayDang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoiDungChiTiet).HasColumnType("ntext");
            entity.Property(e => e.NoiDungTomTat).HasMaxLength(500);
            entity.Property(e => e.TieuDe).HasMaxLength(200);
        });

        modelBuilder.Entity<BenhNhan>(entity =>
        {
            entity.HasKey(e => e.MaBenhNhan);

            entity.ToTable("BenhNhan");

            entity.HasIndex(e => e.MaLoaiBenhNhan, "IX_BenhNhan_MaLoaiBenhNhan");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaSoBaoHiem).HasMaxLength(50);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);

            entity.HasOne(d => d.MaLoaiBenhNhanNavigation).WithMany(p => p.BenhNhans)
                .HasForeignKey(d => d.MaLoaiBenhNhan)
                .HasConstraintName("FK_BenhNhan_LoaiBenhNhan");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet);

            entity.ToTable("ChiTietHoaDon");

            entity.HasIndex(e => e.MaHoaDon, "IX_ChiTietHoaDon_MaHoaDon");

            entity.HasIndex(e => e.MaLoThuoc, "IX_ChiTietHoaDon_MaNhapKho");

            entity.HasIndex(e => e.MaThuoc, "IX_ChiTietHoaDon_MaThuoc");

            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenDichVu).HasMaxLength(100);

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietHoaDon_HoaDon");

            entity.HasOne(d => d.MaLoThuocNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaLoThuoc)
                .HasConstraintName("FK_ChiTietHoaDon_LoThuoc");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaThuoc)
                .HasConstraintName("FK_ChiTietHoaDon_Thuoc");
        });

        modelBuilder.Entity<ChuyenKhoaBacSi>(entity =>
        {
            entity.HasKey(e => e.MaChuyenKhoa);

            entity.ToTable("ChuyenKhoaBacSi");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenChuyenKhoa).HasMaxLength(100);
        });

        modelBuilder.Entity<DanhMucThuoc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc);

            entity.ToTable("DanhMucThuoc");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
        });

        modelBuilder.Entity<DonVi>(entity =>
        {
            entity.HasKey(e => e.MaDonVi);

            entity.ToTable("DonVi");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenDonVi).HasMaxLength(50);
        });

        modelBuilder.Entity<HoSoBenhAn>(entity =>
        {
            entity.HasKey(e => e.MaHoSo);

            entity.ToTable("HoSoBenhAn");

            entity.HasIndex(e => e.MaBacSi, "IX_HoSoBenhAn_MaBacSi");

            entity.HasIndex(e => e.MaBenhNhan, "IX_HoSoBenhAn_MaBenhNhan");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.HoSoBenhAns)
                .HasForeignKey(d => d.MaBacSi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoSoBenhAn_BacSi");

            entity.HasOne(d => d.MaBenhNhanNavigation).WithMany(p => p.HoSoBenhAns)
                .HasForeignKey(d => d.MaBenhNhan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoSoBenhAn_BenhNhan");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon);

            entity.ToTable("HoaDon");

            entity.HasIndex(e => e.MaBenhNhan, "IX_HoaDon_MaBenhNhan");

            entity.HasIndex(e => e.MaHoSoBenhAn, "IX_HoaDon_MaHoSoBenhAn");

            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.GiamGia).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LoaiHoaDon).HasMaxLength(20);
            entity.Property(e => e.NgayLapHoaDon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Thue).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(20);

            entity.HasOne(d => d.MaBenhNhanNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaBenhNhan)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_HoaDon_BenhNhan");

            entity.HasOne(d => d.MaHoSoBenhAnNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaHoSoBenhAn)
                .HasConstraintName("FK_HoaDon_HoSoBenhAn");

            entity.HasOne(d => d.NhanVienKeDonNavigation).WithMany(p => p.HoaDonNhanVienKeDonNavigations)
                .HasForeignKey(d => d.NhanVienKeDon)
                .HasConstraintName("FK_HoaDon_NhanVienKeDon");

            entity.HasOne(d => d.NhanVienXacNhanNavigation).WithMany(p => p.HoaDonNhanVienXacNhanNavigations)
                .HasForeignKey(d => d.NhanVienXacNhan)
                .HasConstraintName("FK_HoaDon_NhanVienXacNhan");
        });

        modelBuilder.Entity<LichHen>(entity =>
        {
            entity.HasKey(e => e.MaLichHen);

            entity.ToTable("LichHen");

            entity.HasIndex(e => e.MaBacSi, "IX_LichHen_MaBacSi");

            entity.HasIndex(e => e.MaBenhNhan, "IX_LichHen_MaBenhNhan");

            entity.HasIndex(e => e.MaLoaiLichHen, "IX_LichHen_MaLoaiLichHen");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.MaLoaiLichHen).HasDefaultValue(1);
            entity.Property(e => e.NgayHen).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(20);

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.LichHens)
                .HasForeignKey(d => d.MaBacSi)
                .HasConstraintName("FK_LichHen_BacSi");

            entity.HasOne(d => d.MaBenhNhanNavigation).WithMany(p => p.LichHens)
                .HasForeignKey(d => d.MaBenhNhan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichHen_BenhNhan");

            entity.HasOne(d => d.MaLoaiLichHenNavigation).WithMany(p => p.LichHens)
                .HasForeignKey(d => d.MaLoaiLichHen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichHen_LoaiLichHen");
        });

        modelBuilder.Entity<LoThuoc>(entity =>
        {
            entity.HasKey(e => e.MaLoThuoc);

            entity.ToTable("LoThuoc");

            entity.HasIndex(e => e.MaThuoc, "IX_NhapKho_MaThuoc");

            entity.Property(e => e.DangBan).HasDefaultValue(false);
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[DonGia])", false)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.TiLeLoiNhuan).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.LoThuocs)
                .HasForeignKey(d => d.MaNhaCungCap)
                .HasConstraintName("FK_LoThuoc_NhaCungCap");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.LoThuocs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoThuoc_Thuoc");

            entity.HasOne(d => d.NguoiNhapNavigation).WithMany(p => p.LoThuocs)
                .HasForeignKey(d => d.NguoiNhap)
                .HasConstraintName("FK_LoThuoc_NhanVien");
        });

        modelBuilder.Entity<LoaiBenhNhan>(entity =>
        {
            entity.HasKey(e => e.MaLoaiBenhNhan);

            entity.ToTable("LoaiBenhNhan");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.GiamGia)
                .HasDefaultValue(0.0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<LoaiLichHen>(entity =>
        {
            entity.HasKey(e => e.MaLoaiLichHen);

            entity.ToTable("LoaiLichHen");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.GiaTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCungCap);

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.DangHoatDong).HasDefaultValue(false);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MaSoNhaCungCap).HasMaxLength(20);
            entity.Property(e => e.MaSoThue).HasMaxLength(20);
            entity.Property(e => e.NguoiLienHe).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenNhaCungCap).HasMaxLength(100);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien);

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.MaChuyenKhoa, "IX_BacSi_MaChuyenKhoa");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HinhAnh).HasMaxLength(500);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.LichLamViec).HasMaxLength(255);
            entity.Property(e => e.LinkChungChi).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);

            entity.HasOne(d => d.MaChuyenKhoaNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaChuyenKhoa)
                .HasConstraintName("FK_BacSi_ChuyenKhoa");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaVaiTro)
                .HasConstraintName("FK_NhanVien_VaiTro");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.TenDangNhap);

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.MaBacSi, "IX_TaiKhoan_MaBacSi");

            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.DaDangNhap).HasDefaultValue(false);
            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.MatKhau).HasMaxLength(256);
            entity.Property(e => e.VaiTro).HasMaxLength(50);

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaBacSi)
                .HasConstraintName("FK_TaiKhoan_BacSi");
        });

        modelBuilder.Entity<TaiKhoanNguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C65299D3E1C19");

            entity.ToTable("TaiKhoanNguoiDung");

            entity.HasIndex(e => e.SoDienThoai, "UQ__TaiKhoan__0389B7BD7B53C440").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MatKhauHash).HasMaxLength(500);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaBenhNhanNavigation).WithMany(p => p.TaiKhoanNguoiDungs)
                .HasForeignKey(d => d.MaBenhNhan)
                .HasConstraintName("FK_TaiKhoanNguoiDung_BenhNhan");
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.HasKey(e => e.MaThuoc);

            entity.ToTable("Thuoc");

            entity.HasIndex(e => e.MaDanhMuc, "IX_Thuoc_MaDanhMuc");

            entity.HasIndex(e => e.MaDonVi, "IX_Thuoc_MaDonVi");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.MaQr)
                .IsUnicode(false)
                .HasColumnName("MaQR");
            entity.Property(e => e.MaVach).IsUnicode(false);
            entity.Property(e => e.TenThuoc).HasMaxLength(100);

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.Thuocs)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("FK_Thuoc_DanhMucThuoc");

            entity.HasOne(d => d.MaDonViNavigation).WithMany(p => p.Thuocs)
                .HasForeignKey(d => d.MaDonVi)
                .HasConstraintName("FK_Thuoc_DonVi");
        });

        modelBuilder.Entity<TonKho>(entity =>
        {
            entity.HasKey(e => e.MaTonKho);

            entity.ToTable("TonKho");

            entity.HasIndex(e => e.MaThuoc, "IX_KhoThuoc_MaThuoc");

            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.TonKhos)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TonKho_Thuoc");
        });

        modelBuilder.Entity<TonKhoTheoThang>(entity =>
        {
            entity.HasKey(e => e.MaTonThang).HasName("PK__TonKhoTh__3472998FFA646AE6");

            entity.ToTable("TonKhoTheoThang");

            entity.HasIndex(e => new { e.MaThuoc, e.ThangNam }, "UQ_TonKhoTheoThang_ThangThuoc").IsUnique();

            entity.Property(e => e.NgayGhiNhan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThangNam)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.TonKhoTheoThangs)
                .HasForeignKey(d => d.MaThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TonKhoThe__MaThu__07C12930");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro");

            entity.ToTable("VaiTro");

            entity.Property(e => e.DaXoa).HasDefaultValue(false);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenVaiTro).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
