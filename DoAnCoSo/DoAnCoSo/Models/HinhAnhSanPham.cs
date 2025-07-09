using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class HinhAnhSanPham
    {
        [Key]
        public int MaHinhAnhSanPham { get; set; }

        public string LoaiHinhAnh { get; set; }

        public int? MaSanPhamID { get; set; }
        public SanPham? SanPham { get; set; }
    }


}
