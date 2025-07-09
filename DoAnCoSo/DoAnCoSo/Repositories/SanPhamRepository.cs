using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly SeafoodDbContext _context;

        public SanPhamRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _context.SanPhams.ToListAsync();
        }

        public async Task<IEnumerable<SanPham>> GetAllWithDetailsAsync()
        {
            return await _context.SanPhams
                .Include(s => s.ChiTietPhieuNhaps)
                    .ThenInclude(ct => ct.PhieuNhapHang)
                .ToListAsync();
        }

        public async Task<SanPham?> GetByIdAsync(int id)
        {
            return await _context.SanPhams
                .Include(s => s.ChiTietPhieuNhaps)
                    .ThenInclude(ct => ct.PhieuNhapHang)
                .FirstOrDefaultAsync(s => s.MaSanPham == id);
        }

        public async Task<IEnumerable<SanPham>> SearchAsync(string searchTerm)
        {
            return await _context.SanPhams
                .Where(s => s.TenSanPham.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<SanPham>> SearchWithDetailsAsync(string searchTerm)
        {
            return await _context.SanPhams
                .Include(s => s.ChiTietPhieuNhaps)
                    .ThenInclude(ct => ct.PhieuNhapHang)
                .Where(s => s.TenSanPham.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task AddAsync(SanPham sanPham)
        {
            await _context.SanPhams.AddAsync(sanPham);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SanPham sanPham)
        {
            _context.SanPhams.Update(sanPham);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
        }
    }
}
