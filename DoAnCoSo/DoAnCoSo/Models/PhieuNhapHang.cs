using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnCoSo.Models
{
    public class PhieuNhapHang
    {
        [Key]
        public int MaPhieuNhap { get; set; }

        public decimal TongTien { get; set; }
        public int TongSoLuong { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        public int? MaNhaCungCapID { get; set; }
        public NhaCungCap? NhaCungCap { get; set; }

        // Trạng thái xác nhận của phiếu nhập
        public bool DaXacNhan { get; set; } = false;
            
        // Thời gian xác nhận
        [DataType(DataType.DateTime)]
        public DateTime? ThoiGianXacNhan { get; set; }

        public string? AnhSanPham { get; set; }

        public ICollection<ChiTietPhieuNhap>? ChiTietPhieuNhaps { get; set; }
    }
}
