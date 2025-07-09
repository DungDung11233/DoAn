using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IHinhAnhSanPhamRepository
    {
        Task<IEnumerable<HinhAnhSanPham>> GetAllAsync();
        Task<HinhAnhSanPham> GetByIdAsync(int maHinhAnhSanPham);
        Task AddAsync(HinhAnhSanPham hinhAnhSanPham);
        Task UpdateAsync(HinhAnhSanPham hinhAnhSanPham);
        Task DeleteAsync(int maHinhAnhSanPham);
    }
}
