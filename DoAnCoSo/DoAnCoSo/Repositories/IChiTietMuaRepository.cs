using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IChiTietMuaRepository
    {
        Task<IEnumerable<ChiTietMua>> GetAllAsync();
        Task<ChiTietMua> GetByIdAsync(int maSanPham, int maMua);
        Task AddAsync(ChiTietMua chiTietMua);
        Task UpdateAsync(ChiTietMua chiTietMua);
        Task DeleteAsync(int maSanPham, int maMua);
    }
}
