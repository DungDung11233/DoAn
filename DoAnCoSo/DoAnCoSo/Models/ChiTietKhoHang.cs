using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class ChiTietKhoHang
    {
        public int? MaSanPhamID { get; set; }
        public SanPham? SanPham { get; set; }

        public int? MaNhaKhoID { get; set; }
        public NhaKho? NhaKho { get; set; }

        public int SoLuong { get; set; }
        public DateTime NgayNhap { get; set; }
    }


}
