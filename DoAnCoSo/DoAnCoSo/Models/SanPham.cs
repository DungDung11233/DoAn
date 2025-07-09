using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models
{
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int? SoLuong { get; set; }
        public string? NguonGoc { get; set; }
        public decimal GiaTheoKG { get; set; }
        public DateTime NgayThuHoach { get; set; }
        public string LoaiBaoQuan { get; set; }
        public string? MoTa { get; set; }
        public string? TinhTrang { get; set; }
        public string? ImageUrl { get; set; }
        public int? DanhMucID { get; set; }
        public DanhMuc? DanhMuc { get; set; }
        public int? MaLoaiID { get; set; }
        public LoaiSanPham? LoaiSanPham { get; set; }

        public ICollection<ChiTietPhieuNhap>? ChiTietPhieuNhaps { get; set; }
        public ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
        public ICollection<ChiTietKhoHang>? ChiTietKhoHangs { get; set; }
        public ICollection<DanhGia>? DanhGias { get; set; }
        public List<HinhAnhSanPham>? HinhAnhSanPhams { get; set; }
        public ICollection<ChiTietMua>? ChiTietMuas { get; set; }
    }



}
