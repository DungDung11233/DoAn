using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.Models
{
    public class Mua
    {
        [Key]
        public int MaMua { get; set; }
        public string TenMua { get; set; }
        public DateTime ThoiGianVaoMua { get; set; }
        public DateTime ThoiGianHetMua { get; set; }


        public ICollection<ChiTietMua>? ChiTietMuas { get; set; }
    }


}
