using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class TrangThaiDonHang
    {
        [Key]
        public int MaTrangThai { get; set; }
        public string? TenTrangThai { get; set; }
        public string? GhiChu { get; set; }
        public ICollection<ChiTietTrangThaiDonHang>? ChiTietTrangThaiDonHangs { get; set; }
    }
}
