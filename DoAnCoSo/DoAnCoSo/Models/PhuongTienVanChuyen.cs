using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class PhuongTienVanChuyen
    {
        [Key]
        public int MaPhuongTien { get; set; }

        [Required]
        [StringLength(100)]
        public string LoaiPhuongTien { get; set; } = string.Empty;

        public double TrongTai { get; set; }

        // Navigation property
        public ICollection<PhieuVanChuyen>? PhieuVanChuyens { get; set; }
    }
}
