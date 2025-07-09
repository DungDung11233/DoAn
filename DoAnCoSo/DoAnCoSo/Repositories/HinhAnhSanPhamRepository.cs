using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class HinhAnhSanPhamRepository : IHinhAnhSanPhamRepository
    {
        private readonly SeafoodDbContext _context;

        public HinhAnhSanPhamRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HinhAnhSanPham>> GetAllAsync()
        {
            return await _context.HinhAnhSanPhams
                                 .Include(h => h.SanPham)
                                 .ToListAsync();
        }

        public async Task<HinhAnhSanPham> GetByIdAsync(int maHinhAnhSanPham)
        {
            return await _context.HinhAnhSanPhams
                                 .Include(h => h.SanPham)
                                 .FirstOrDefaultAsync(h => h.MaHinhAnhSanPham == maHinhAnhSanPham);
        }

        public async Task AddAsync(HinhAnhSanPham hinhAnhSanPham)
        {
            await _context.HinhAnhSanPhams.AddAsync(hinhAnhSanPham);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HinhAnhSanPham hinhAnhSanPham)
        {
            _context.HinhAnhSanPhams.Update(hinhAnhSanPham);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maHinhAnhSanPham)
        {
            var hinhAnh = await _context.HinhAnhSanPhams.FindAsync(maHinhAnhSanPham);
            if (hinhAnh != null)
            {
                _context.HinhAnhSanPhams.Remove(hinhAnh);
                await _context.SaveChangesAsync();
            }
        }
    }
}
