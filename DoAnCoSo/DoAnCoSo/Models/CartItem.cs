namespace DoAnCoSo.Models
{
    public class CartItem
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string? NguonGoc { get; set; }
        public decimal GiaTheoKG { get; set; }
        public int SoLuong { get; set; }
        public string? ImageUrl { get; set; }
    }
}
