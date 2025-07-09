using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DoAnCoSo.ViewModels
{
    public class CreateShippingViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một đơn hàng")]
        [Display(Name = "Các đơn hàng")]
        public List<int> MaDonHangs { get; set; } = new List<int>();

        [Required(ErrorMessage = "Vui lòng chọn phương tiện vận chuyển")]
        [Display(Name = "Phương tiện vận chuyển")]
        public int MaPhuongTienID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhân viên giao hàng")]
        [Display(Name = "Nhân viên giao hàng")]
        public int MaNhanVienID { get; set; }
    }

    public class PendingDeliveryViewModel
    {
        public int MaDonHang { get; set; }
        public int MaVanChuyen { get; set; }
        public DateTime NgayVanChuyen { get; set; }
        public string TrangThai { get; set; }
        public string LoaiSanPham { get; set; }
        public DateTime NgayDuKienGiaoHang { get; set; }
        public string PhuongTien { get; set; }
        public string NhanVien { get; set; }
    }
} 