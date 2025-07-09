using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class PhieuVanChuyen
    {
        [Key]
        public int MaVanChuyen { get; set; }

        [Required]
        public int MaPhuongTienID { get; set; }

        [Required]
        public int? MaNhanVienID { get; set; }
        public NhanVien? NhanVien { get; set; }

        public DateTime NgayVanChuyen { get; set; }

        // Navigation properties
        public PhuongTienVanChuyen? PhuongTienVanChuyen { get; set; } = null!;

        public ICollection<ChiTietVanChuyen>? ChiTietVanChuyens { get; set; }
    }

}
