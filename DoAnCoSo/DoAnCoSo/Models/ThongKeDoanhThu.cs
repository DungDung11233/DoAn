using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class ThongKeDoanhThu
    {
        [Key]
        public int MaThongKe { get; set; }

        [DataType(DataType.Date)]
        public DateTime ThoiGian { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DoanhThu { get; set; }

        // Foreign Key
        public int MaHoaDonID { get; set; }

        [ForeignKey("MaHoaDonID")]
        public HoaDon? HoaDon { get; set; }
    }
}
