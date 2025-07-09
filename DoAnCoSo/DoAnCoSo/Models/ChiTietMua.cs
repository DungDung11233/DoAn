using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class ChiTietMua
    {
        public int? MaSanPhamID { get; set; }
        public SanPham? SanPham { get; set; }

        public int? MaMuaID { get; set; }
        public Mua? Mua { get; set; }
        public int SoLuongTheoMua { get; set; }
    }

}
