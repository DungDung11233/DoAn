using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IPhuongThucThanhToanRepository
    {
        Task<IEnumerable<PhuongThucThanhToan>> GetAllAsync();
        Task<PhuongThucThanhToan> GetByIdAsync(int maPTTT);
        Task AddAsync(PhuongThucThanhToan phuongThucThanhToan);
        Task UpdateAsync(PhuongThucThanhToan phuongThucThanhToan);
        Task DeleteAsync(int maPTTT);
    }
}
