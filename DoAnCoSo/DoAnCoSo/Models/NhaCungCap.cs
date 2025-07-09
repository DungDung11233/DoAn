using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class NhaCungCap
    {
        [Key]
        public int MaNhaCungCap { get; set; }

        [Required, StringLength(100)]
        public string TenNCC { get; set; } = string.Empty;

        public string? DiaChiNCC { get; set; }
        public string? Email { get; set; }
        public string? SDT { get; set; }

        public string? UserID { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<PhieuNhapHang>? PhieuNhapHangs { get; set; }
    }
}
