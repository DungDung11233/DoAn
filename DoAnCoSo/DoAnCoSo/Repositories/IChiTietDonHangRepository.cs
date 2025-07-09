using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IChiTietDonHangRepository
    {
        Task<IEnumerable<ChiTietDonHang>> GetAllAsync();
        Task<ChiTietDonHang> GetByIdAsync(int maSanPham, int maDonHang);
        Task AddAsync(ChiTietDonHang chiTietDonHang);
        Task UpdateAsync(ChiTietDonHang chiTietDonHang);
        Task DeleteAsync(int maSanPham, int maDonHang);
    }
}
