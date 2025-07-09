using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class NhaKho
    {
        [Key]
        public int MaNhaKho { get; set; }
        public string TenNhaKho { get; set; }
        public string DiaChi { get; set; }
        public string LoaiKho { get; set; }

        public ICollection<ChiTietKhoHang>? ChiTietKhoHangs { get; set; }
    }


}
