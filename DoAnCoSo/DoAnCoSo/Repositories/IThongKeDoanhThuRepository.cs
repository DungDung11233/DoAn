using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IThongKeDoanhThuRepository
    {
        Task<IEnumerable<ThongKeDoanhThu>> GetAllAsync();
        Task<ThongKeDoanhThu> GetByIdAsync(int id);
        Task<IEnumerable<ThongKeDoanhThu>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddAsync(ThongKeDoanhThu thongKe);
        Task UpdateAsync(ThongKeDoanhThu thongKe);
    }
}
