using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IKhachHangRepository
    {
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang> GetByIdAsync(int maNguoiDung);
        Task AddAsync(KhachHang nguoiDung);
        Task UpdateAsync(KhachHang nguoiDung);
        Task DeleteAsync(int maNguoiDung);
    }
}
