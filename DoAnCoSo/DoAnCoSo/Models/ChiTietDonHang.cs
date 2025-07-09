using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class ChiTietDonHang
    {
        
        public int? MaSanPhamID { get; set; }
        public SanPham? SanPham { get; set; }

        public int? MaDonHangID { get; set; }
        public DonHang? DonHang { get; set; }

        public int SoLuong { get; set; }
       
    }


}
