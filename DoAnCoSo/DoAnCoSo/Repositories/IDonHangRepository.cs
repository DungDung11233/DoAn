using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IDonHangRepository
    {
        Task<IEnumerable<DonHang>> GetAllAsync();
        Task<DonHang> GetByIdAsync(int id);
        Task AddAsync(DonHang donHang);
        Task UpdateAsync(DonHang donHang);
        Task DeleteAsync(int id);
    }
}
