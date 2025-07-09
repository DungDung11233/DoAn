using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IMuaRepository
    {
        Task<IEnumerable<Mua>> GetAllAsync();
        Task<Mua> GetByIdAsync(int maMua);
        Task AddAsync(Mua mua);
        Task UpdateAsync(Mua mua);
        Task DeleteAsync(int maMua);
    }
}
