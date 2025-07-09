using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class SupplierAdditionalInfo
    {
        [Required(ErrorMessage = "Vui lòng nhập tên nhà cung cấp")]
        [Display(Name = "Tên nhà cung cấp")]
        [StringLength(100)]
        public string TenNCC { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string DiaChiNCC { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
} 