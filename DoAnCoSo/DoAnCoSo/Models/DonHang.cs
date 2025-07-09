using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models
{
    public class DonHang
    {
          [Key]
        public int MaDonHang { get; set; }
        public DateTime NgayDatHang { get; set; }

        public decimal TongSoTien { get; set; }

        public string? DiaChiGiaoHang { get; set; }
        public string? GhiChu { get; set; }
        public int? MaKhachHangID { get; set; }
        public KhachHang? KhachHang { get; set; }


        public int? MaGiamGiaID { get; set; }
        public GiamGia? GiamGia { get; set; }

        public int? MaPTTTID { get; set; }
        public PhuongThucThanhToan? PhuongThucThanhToan { get; set; }

        public int? MaTrangThaiID { get; set; }
        public TrangThaiThanhToan? TrangThaiThanhToan { get; set; }

        //public string? UserID { get; set; }
        //public ApplicationUser? User { get; set; }

        public ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
        public ICollection<ChiTietTrangThaiDonHang>? ChiTietTrangThaiDonHangs { get; set; }
        public ICollection<ChiTietVanChuyen>? ChiTietVanChuyens { get; set; }
        public ICollection<HoaDon>? HoaDons { get; set; }

    }

}
