using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IChiTietPhieuNhapRepository
    {
        Task<IEnumerable<ChiTietPhieuNhap>> GetAllAsync();
        Task<ChiTietPhieuNhap?> GetByIdAsync(int maPhieuNhap, int maSanPham);
        Task AddAsync(ChiTietPhieuNhap entity);
        Task UpdateAsync(ChiTietPhieuNhap entity);
        Task DeleteAsync(int maPhieuNhap, int maSanPham);
    }
}
