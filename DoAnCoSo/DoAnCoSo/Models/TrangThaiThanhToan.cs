using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models
{
    public class TrangThaiThanhToan
    {
        [Key]
        public int MaTrangThai { get; set; }

        [Required]
        [StringLength(100)]
        public string TenTrangThai { get; set; } = string.Empty;

        // Navigation property nếu có liên kết với DonHang hoặc bảng khác
        public ICollection<DonHang>? DonHangs { get; set; }
    }
}
