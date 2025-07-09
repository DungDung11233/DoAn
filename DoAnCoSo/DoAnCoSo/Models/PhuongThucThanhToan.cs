using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class PhuongThucThanhToan
    {
        [Key]
        public int MaPTTT { get; set; }
        public string TenPTTT { get; set; }

        public ICollection<DonHang>? DonHangs { get; set; }


    }
}

