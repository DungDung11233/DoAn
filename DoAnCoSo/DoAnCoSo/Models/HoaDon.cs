using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models
{
    public class HoaDon
    {
        [Key]
        public int MaHoaDon { get; set; }

        [Required]
        public decimal TongSoTien { get; set; }

        [Required]
        public DateTime NgayTaoHoaDon { get; set; }

        public int? MaNhanVienID { get; set; }
        public NhanVien? NhanVien { get; set; }


        public int? MaKhachHangID { get; set; }
        public KhachHang? KhachHang { get; set; }

        public int? MaDonHangID { get; set; }
        [ForeignKey("MaDonHangID")]
        public DonHang? DonHang { get; set; }

        public ICollection<ThongKeDoanhThu>? ThongKeDoanhThus { get; set; }
    }
}