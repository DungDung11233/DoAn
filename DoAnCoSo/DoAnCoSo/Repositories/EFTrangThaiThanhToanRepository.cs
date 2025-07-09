using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Repositories
{
    public class EFTrangThaiThanhToanRepository : ITrangThaiThanhToanRepository
    {
        private readonly SeafoodDbContext _context;

        public EFTrangThaiThanhToanRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrangThaiThanhToan>> GetAllAsync()
        {
            return await _context.TrangThaiThanhToans.ToListAsync();
        }

        public async Task<TrangThaiThanhToan?> GetByIdAsync(int id)
        {
            return await _context.TrangThaiThanhToans.FindAsync(id);
        }

        public async Task AddAsync(TrangThaiThanhToan entity)
        {
            _context.TrangThaiThanhToans.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrangThaiThanhToan entity)
        {
            _context.TrangThaiThanhToans.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TrangThaiThanhToans.FindAsync(id);
            if (entity != null)
            {
                _context.TrangThaiThanhToans.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

