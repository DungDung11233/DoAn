using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class DanhGiaRepository : IDanhGiaRepository
    {
        private readonly SeafoodDbContext _context;

        public DanhGiaRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DanhGia>> GetAllAsync()
        {
            return await _context.DanhGias
                                 .Include(d => d.SanPham)
                                 .Include(d => d.KhachHang)
                                 .ToListAsync();
        }

        public async Task<DanhGia> GetByIdAsync(int maDanhGia)
        {
            return await _context.DanhGias
                                 .Include(d => d.SanPham)
                                 .Include(d => d.KhachHang)
                                 .FirstOrDefaultAsync(d => d.MaDanhGia == maDanhGia);
        }

        public async Task AddAsync(DanhGia danhGia)
        {
            await _context.DanhGias.AddAsync(danhGia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DanhGia danhGia)
        {
            _context.DanhGias.Update(danhGia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maDanhGia)
        {
            var danhGia = await _context.DanhGias.FirstOrDefaultAsync(d => d.MaDanhGia == maDanhGia);
            if (danhGia != null)
            {
                _context.DanhGias.Remove(danhGia);
                await _context.SaveChangesAsync();
            }
        }
    }
}
