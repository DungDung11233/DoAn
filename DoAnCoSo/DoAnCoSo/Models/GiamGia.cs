using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class GiamGia
    {
        [Key]
        public int MaGiamGia { get; set; }
        public string Code { get; set; } = null!;
        public int SoLuong { get; set; }
        public decimal? GiamTheoSoTien { get; set; } // Giảm theo tiền
        public double? GiamTheoPhanTram { get; set; } // Giảm theo %
        public DateTime NgayApDung { get; set; }
        public int SoLanToiDaDungMa { get; set; } = 1; // Số lần tối đa mã có thể sử dụng, mặc định là 1
        public int SoLanDungMa { get; set; } = 0; // Số lần mã đã được sử dụng

        public ICollection<DonHang>? DonHangs { get; set; }
    }

}
