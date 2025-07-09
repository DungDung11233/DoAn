using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IChiTietKhoHangRepository
    {
        Task<IEnumerable<ChiTietKhoHang>> GetAllAsync();
        Task<ChiTietKhoHang> GetByIdAsync(int maSanPham, int maNhaKho);
        Task AddAsync(ChiTietKhoHang chiTietKhoHang);
        Task UpdateAsync(ChiTietKhoHang chiTietKhoHang);
        Task DeleteAsync(int maSanPham, int maNhaKho);
    }
}
