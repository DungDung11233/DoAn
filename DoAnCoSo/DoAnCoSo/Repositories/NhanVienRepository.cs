using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly SeafoodDbContext _context;

        public NhanVienRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhanVien>> GetAllAsync()
        {
            return await _context.NhanViens
                                 .Include(n => n.HoaDons)
                                 .Include(n => n.PhieuVanChuyens)
                                 .ToListAsync();
        }

        public async Task<NhanVien> GetByIdAsync(int maNhanVien)
        {
            return await _context.NhanViens
                                 .Include(n => n.HoaDons)
                                 .Include(n => n.PhieuVanChuyens)
                                 .FirstOrDefaultAsync(n => n.MaNhanVien == maNhanVien);
        }

        public async Task AddAsync(NhanVien nhanVien)
        {
            await _context.NhanViens.AddAsync(nhanVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhanVien nhanVien)
        {
            _context.NhanViens.Update(nhanVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maNhanVien)
        {
            var nhanVien = await _context.NhanViens.FindAsync(maNhanVien);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
                await _context.SaveChangesAsync();
            }
        }
    }
}
