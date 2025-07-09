using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly SeafoodDbContext _context;

        public HoaDonRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HoaDon>> GetAllAsync()
        {
            return await _context.HoaDons
                                 .Include(h => h.NhanVien)
                                 .Include(h => h.KhachHang)
                                 .Include(h => h.DonHang)
                                 .ToListAsync();
        }

        public async Task<HoaDon> GetByIdAsync(int maHoaDon)
        {
            return await _context.HoaDons
                                 .Include(h => h.NhanVien)
                                 .Include(h => h.KhachHang)
                                 .Include(h => h.DonHang)
                                 .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
        }

        public async Task AddAsync(HoaDon hoaDon)
        {
            await _context.HoaDons.AddAsync(hoaDon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HoaDon hoaDon)
        {
            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maHoaDon)
        {
            var hoaDon = await _context.HoaDons.FindAsync(maHoaDon);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
                await _context.SaveChangesAsync();
            }
        }
    }
}
