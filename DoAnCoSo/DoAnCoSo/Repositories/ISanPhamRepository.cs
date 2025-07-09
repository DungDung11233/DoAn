using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface ISanPhamRepository
    {
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<IEnumerable<SanPham>> GetAllWithDetailsAsync();
        Task<SanPham?> GetByIdAsync(int id);
        Task<IEnumerable<SanPham>> SearchAsync(string searchTerm);
        Task<IEnumerable<SanPham>> SearchWithDetailsAsync(string searchTerm);
        Task AddAsync(SanPham sanPham);
        Task UpdateAsync(SanPham sanPham);
        Task DeleteAsync(int id);
    }
}
