using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Repositories
{
    public class PhuongTienVanChuyenRepository : IPhuongTienVanChuyenRepository
    {
        private readonly SeafoodDbContext _context;

        public PhuongTienVanChuyenRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhuongTienVanChuyen>> GetAllAsync()
        {
            return await _context.PhuongTienVanChuyens
                                 .ToListAsync();
        }

        public async Task<PhuongTienVanChuyen> GetByIdAsync(int maPhuongTien)
        {
            return await _context.PhuongTienVanChuyens
                                 .FirstOrDefaultAsync(p => p.MaPhuongTien == maPhuongTien);
        }

        public async Task AddAsync(PhuongTienVanChuyen phuongTien)
        {
            await _context.PhuongTienVanChuyens.AddAsync(phuongTien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhuongTienVanChuyen phuongTien)
        {
            _context.PhuongTienVanChuyens.Update(phuongTien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maPhuongTien)
        {
            var phuongTien = await _context.PhuongTienVanChuyens.FindAsync(maPhuongTien);
            if (phuongTien != null)
            {
                _context.PhuongTienVanChuyens.Remove(phuongTien);
                await _context.SaveChangesAsync();
            }
        }
    }
} 