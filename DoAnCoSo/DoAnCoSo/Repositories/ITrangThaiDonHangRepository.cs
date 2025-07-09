using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface ITrangThaiDonHangRepository
    {
        Task<IEnumerable<TrangThaiDonHang>> GetAllAsync();
        Task<TrangThaiDonHang?> GetByIdAsync(int maTrangThai);
        Task AddAsync(TrangThaiDonHang trangThaiDonHang);
        Task UpdateAsync(TrangThaiDonHang trangThaiDonHang);
        Task DeleteAsync(int maTrangThai);
    }
}
