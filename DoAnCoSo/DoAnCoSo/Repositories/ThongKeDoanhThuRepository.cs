using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Repositories
{
    public class ThongKeDoanhThuRepository : IThongKeDoanhThuRepository
    {
        private readonly SeafoodDbContext _context;

        public ThongKeDoanhThuRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ThongKeDoanhThu>> GetAllAsync()
        {
            return await _context.ThongKeDoanhThus
                .Include(t => t.HoaDon)
                .ToListAsync();
        }

        public async Task<ThongKeDoanhThu> GetByIdAsync(int id)
        {
            return await _context.ThongKeDoanhThus
                .Include(t => t.HoaDon)
                .FirstOrDefaultAsync(t => t.MaThongKe == id);
        }

        public async Task<IEnumerable<ThongKeDoanhThu>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ThongKeDoanhThus
                .Where(t => t.ThoiGian >= startDate && t.ThoiGian <= endDate)
                .Include(t => t.HoaDon)
                .ToListAsync();
        }

        public async Task AddAsync(ThongKeDoanhThu thongKe)
        {
            await _context.ThongKeDoanhThus.AddAsync(thongKe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ThongKeDoanhThu thongKe)
        {
            _context.ThongKeDoanhThus.Update(thongKe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var thongKe = await _context.ThongKeDoanhThus.FindAsync(id);
            if (thongKe != null)
            {
                _context.ThongKeDoanhThus.Remove(thongKe);
                await _context.SaveChangesAsync();
            }
        }
    }
}
