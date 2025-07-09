using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IPhuongTienVanChuyenRepository
    {
        Task<IEnumerable<PhuongTienVanChuyen>> GetAllAsync();
        Task<PhuongTienVanChuyen> GetByIdAsync(int maPhuongTien);
        Task AddAsync(PhuongTienVanChuyen phuongTien);
        Task UpdateAsync(PhuongTienVanChuyen phuongTien);
        Task DeleteAsync(int maPhuongTien);
    }
}
