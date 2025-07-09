using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class NhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayTuyenDung { get; set; }

        public string? UserID { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<HoaDon>? HoaDons { get; set; }
        public ICollection<PhieuVanChuyen>? PhieuVanChuyens { get; set; }
    }


}
