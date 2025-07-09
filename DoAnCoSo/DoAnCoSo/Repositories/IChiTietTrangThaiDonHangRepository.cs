public interface IChiTietTrangThaiDonHangRepository
{
    Task<IEnumerable<ChiTietTrangThaiDonHang>> GetAllAsync();
    Task<ChiTietTrangThaiDonHang> GetByIdAsync(int maDonHang, int maTrangThai);
    Task AddAsync(ChiTietTrangThaiDonHang entity);
    Task UpdateAsync(ChiTietTrangThaiDonHang entity);
    Task DeleteAsync(int maDonHang, int maTrangThai);
}
