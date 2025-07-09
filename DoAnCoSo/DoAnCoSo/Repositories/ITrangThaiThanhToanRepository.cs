using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface ITrangThaiThanhToanRepository
    {
        Task<IEnumerable<TrangThaiThanhToan>> GetAllAsync();
        Task<TrangThaiThanhToan?> GetByIdAsync(int id);
        Task AddAsync(TrangThaiThanhToan entity);
        Task UpdateAsync(TrangThaiThanhToan entity);
        Task DeleteAsync(int id);
    }
}
