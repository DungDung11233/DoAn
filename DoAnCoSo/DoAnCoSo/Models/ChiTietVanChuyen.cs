using DoAnCoSo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class ChiTietVanChuyen
    {
        [Key, Column(Order = 0)]
        public int MaDonHang { get; set; }

        [Key, Column(Order = 1)]
        public int MaVanChuyen { get; set; }

        public string? TrangThai { get; set; }

        // Navigation properties
        public DonHang? DonHang { get; set; }
        public PhieuVanChuyen? PhieuVanChuyen { get; set; }
    }
}
