using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class PhieuVanChuyenRepository : IVanChuyenRepository
    {
        private readonly SeafoodDbContext _context;

        public PhieuVanChuyenRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhieuVanChuyen>> GetAllAsync()
        {
            return await _context.PhieuVanChuyens
                                 .Include(v => v.NhanVien)
                                 .Include(v => v.PhuongTienVanChuyen)
                                 .ToListAsync();
        }

        public async Task<PhieuVanChuyen> GetByIdAsync(int maVanChuyen)
        {
            return await _context.PhieuVanChuyens
                                 .Include(v => v.NhanVien)
                                 .Include(v => v.PhuongTienVanChuyen)
                                 .FirstOrDefaultAsync(v => v.MaVanChuyen == maVanChuyen);
        }

        public async Task AddAsync(PhieuVanChuyen vanChuyen)
        {
            await _context.PhieuVanChuyens.AddAsync(vanChuyen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhieuVanChuyen vanChuyen)
        {
            _context.PhieuVanChuyens.Update(vanChuyen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maVanChuyen)
        {
            var vanChuyen = await _context.PhieuVanChuyens.FindAsync(maVanChuyen);
            if (vanChuyen != null)
            {
                _context.PhieuVanChuyens.Remove(vanChuyen);
                await _context.SaveChangesAsync();
            }
        }
    }
}
