using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface INhaKhoRepository
    {
        Task<IEnumerable<NhaKho>> GetAllAsync();
        Task<NhaKho> GetByIdAsync(int maNhaKho);
        Task AddAsync(NhaKho nhaKho);
        Task UpdateAsync(NhaKho nhaKho);
        Task DeleteAsync(int maNhaKho);
    }
}
