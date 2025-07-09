using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }

        public ICollection<SanPham>? SanPhams { get; set; }

    }

}
