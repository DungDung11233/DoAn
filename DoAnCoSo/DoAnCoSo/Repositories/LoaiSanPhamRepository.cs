using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class LoaiSanPhamRepository : ILoaiSanPhamRepository
    {
        private readonly SeafoodDbContext _context;

        public LoaiSanPhamRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiSanPham>> GetAllAsync()
        {
            return await _context.LoaiSanPhams
                                 .Include(l => l.SanPhams)
                                 .ToListAsync();
        }

        public async Task<LoaiSanPham?> GetByIdAsync(int maLoai)
        {
            return await _context.LoaiSanPhams
                                 .Include(l => l.SanPhams)
                                 .FirstOrDefaultAsync(l => l.MaLoai == maLoai);
        }

        public async Task AddAsync(LoaiSanPham loaiSanPham)
        {
            await _context.LoaiSanPhams.AddAsync(loaiSanPham);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiSanPham loaiSanPham)
        {
            _context.LoaiSanPhams.Update(loaiSanPham);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maLoai)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(maLoai);
            if (loaiSanPham != null)
            {
                _context.LoaiSanPhams.Remove(loaiSanPham);
                await _context.SaveChangesAsync();
            }
        }
    }
}
