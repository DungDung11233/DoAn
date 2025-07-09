namespace DoAnCoSo.Models
{
        public class ShoppingCart
        {
            public List<CartItem> Items { get; set; } = new List<CartItem>();

            public void AddItem(CartItem item)
            {
                var existingItem = Items.FirstOrDefault(i => i.MaSanPham == item.MaSanPham);
                if (existingItem != null)
                {
                    existingItem.SoLuong += item.SoLuong;
                }
                else
                {
                    Items.Add(item);
                }
            }

            public void RemoveItem(int maSanPham)
            {
                Items.RemoveAll(i => i.MaSanPham == maSanPham);
            }

            public decimal GetTotalPrice()
            {
                return Items.Sum(i => i.GiaTheoKG * i.SoLuong);
            }

            public void UpdateItem(int maSanPham, int newSoLuong)
            {
                var item = Items.FirstOrDefault(i => i.MaSanPham == maSanPham);
                if (item != null && newSoLuong > 0)
                {
                    item.SoLuong = newSoLuong;
                }
                else
                {
                    RemoveItem(maSanPham);
                }
            }
        }
    }


