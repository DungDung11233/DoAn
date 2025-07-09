using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IVanChuyenRepository
    {
        Task<IEnumerable<PhieuVanChuyen>> GetAllAsync();
        Task<PhieuVanChuyen> GetByIdAsync(int maVanChuyen);
        Task AddAsync(PhieuVanChuyen vanChuyen);
        Task UpdateAsync(PhieuVanChuyen vanChuyen);
        Task DeleteAsync(int maVanChuyen);
    }
}
