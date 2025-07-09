using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IDanhGiaRepository
    {
        Task<IEnumerable<DanhGia>> GetAllAsync();
        Task<DanhGia> GetByIdAsync(int maDanhGia);
        Task AddAsync(DanhGia danhGia);
        Task UpdateAsync(DanhGia danhGia);
        Task DeleteAsync(int maDanhGia);
    }
}
