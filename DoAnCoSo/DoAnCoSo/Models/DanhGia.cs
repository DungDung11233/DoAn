using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }
        public int XepHang { get; set; }
        public string BinhLuan { get; set; }
        public DateTime NgayDanhGia { get; set; }

        public int? MaSanPhamID { get; set; }
        public SanPham? SanPham { get; set; }

        public int? MaKhachHangID { get; set; }
        public KhachHang? KhachHang { get; set; }

    }

}
