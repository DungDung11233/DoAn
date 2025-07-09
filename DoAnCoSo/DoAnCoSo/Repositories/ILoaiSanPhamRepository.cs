using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface ILoaiSanPhamRepository
    {
        Task<IEnumerable<LoaiSanPham>> GetAllAsync();
        Task<LoaiSanPham?> GetByIdAsync(int maLoai);
        Task AddAsync(LoaiSanPham loaiSanPham);
        Task UpdateAsync(LoaiSanPham loaiSanPham);
        Task DeleteAsync(int maLoai);
    }
}
