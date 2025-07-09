using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IGiamGiaRepository
    {
        Task<IEnumerable<GiamGia>> GetAllAsync();
        Task<GiamGia> GetByIdAsync(int maGiamGia);
        Task AddAsync(GiamGia giamGia);
        Task UpdateAsync(GiamGia giamGia);
        Task DeleteAsync(int maGiamGia);
    }
}
