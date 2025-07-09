using System.ComponentModel.DataAnnotations;
namespace DoAnCoSo.Models
{
    public class LoaiSanPham
    {
        [Key]
        public int MaLoai { get; set; }
        public string? TenLoai { get; set; }
        public string? GhiChu { get; set; }
        public ICollection<SanPham>? SanPhams { get; set; }
    }
}
