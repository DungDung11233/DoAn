using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models
{
    public class ChiTietPhieuNhap
    {
        [Key, Column(Order = 0)]
        public int MaPhieuNhap { get; set; }

        [Key, Column(Order = 1)]
        public int MaSanPham { get; set; }

        public int SoLuongSanPham { get; set; }
        public decimal GiaNhapHang { get; set; }
       

        // Navigation properties
        public PhieuNhapHang PhieuNhapHang { get; set; } = null!;
        public SanPham SanPham { get; set; } = null!;
    }
}
