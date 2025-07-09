using DoAnCoSo.Models;

public class ChiTietTrangThaiDonHang
{
    public int MaDonHang { get; set; }

    public DonHang DonHang { get; set; }

    public int MaTrangThai { get; set; }

    public TrangThaiDonHang TrangThaiDonHang { get; set; }

    public DateTime NgayCapNhat { get; set; }
}
