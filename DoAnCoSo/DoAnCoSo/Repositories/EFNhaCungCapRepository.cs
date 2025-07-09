
using DoAnCoSo.Models;
using DoAnCoSo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Repositories
{
    public class EFNhaCungCapRepository : INhaCungCapRepository
    {
        private readonly SeafoodDbContext _context;

        public EFNhaCungCapRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhaCungCap>> GetAllAsync()
        {
            return await _context.NhaCungCaps.ToListAsync();
        }

        public async Task<NhaCungCap?> GetByIdAsync(int id)
        {
            return await _context.NhaCungCaps.FindAsync(id);
        }

        public async Task AddAsync(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCaps.Add(nhaCungCap);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCaps.Update(nhaCungCap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.NhaCungCaps.FindAsync(id);
            if (entity != null)
            {
                _context.NhaCungCaps.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.NhaCungCaps.AnyAsync(e => e.MaNhaCungCap == id);
        }
    }
}
