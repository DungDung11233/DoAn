using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IDanhMucRepository
    {
        Task<IEnumerable<DanhMuc>> GetAllAsync();
        Task<DanhMuc> GetByIdAsync(int maDanhMuc);
        Task AddAsync(DanhMuc danhMuc);
        Task UpdateAsync(DanhMuc danhMuc);
        Task DeleteAsync(int maDanhMuc);
    }
}
