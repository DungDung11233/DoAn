using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Repositories
{
    public class PhieuNhapHangRepository : IPhieuNhapHangRepository
    {
        private readonly SeafoodDbContext _context;
        private readonly ISynchronizationService _synchronizationService;

        public PhieuNhapHangRepository(SeafoodDbContext context, ISynchronizationService synchronizationService)
        {
            _context = context;
            _synchronizationService = synchronizationService;
        }

        public async Task<IEnumerable<PhieuNhapHang>> GetAllAsync()
        {
            return await _context.PhieuNhapHangs
                .Include(p => p.ChiTietPhieuNhaps)
                .ToListAsync();
        }

        public async Task<PhieuNhapHang> GetByIdAsync(int maPhieuNhap)
        {
            return await _context.PhieuNhapHangs
                .Include(p => p.ChiTietPhieuNhaps)
                .FirstOrDefaultAsync(p => p.MaPhieuNhap == maPhieuNhap);
        }

        public async Task AddAsync(PhieuNhapHang phieuNhapHang)
        {
            _context.PhieuNhapHangs.Add(phieuNhapHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhieuNhapHang phieuNhapHang)
        {
            _context.PhieuNhapHangs.Update(phieuNhapHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maPhieuNhap)
        {
            var phieuNhapHang = await GetByIdAsync(maPhieuNhap);
            if (phieuNhapHang != null)
            {
                _context.PhieuNhapHangs.Remove(phieuNhapHang);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhieuNhapHang>> GetByNhaCungCapIdAsync(int maNhaCungCapId)
        {
            return await _context.PhieuNhapHangs
                .Where(p => p.MaNhaCungCapID == maNhaCungCapId)
                .Include(p => p.ChiTietPhieuNhaps)
                .ToListAsync();
        }

        public async Task AddChiTietAsync(ChiTietPhieuNhap chiTietPhieuNhap)
        {
            _context.ChiTietPhieuNhaps.Add(chiTietPhieuNhap);
            await _context.SaveChangesAsync();
            
            // Synchronize with product system after adding details
            await _synchronizationService.UpdateProductQuantitiesAsync(new List<ChiTietPhieuNhap> { chiTietPhieuNhap });
        }

        public async Task<IEnumerable<ChiTietPhieuNhap>> GetChiTietByPhieuNhapIdAsync(int maPhieuNhap)
        {
            return await _context.ChiTietPhieuNhaps
                .Where(ct => ct.MaPhieuNhap == maPhieuNhap)
                .Include(ct => ct.SanPham)
                .ToListAsync();
        }

        public async Task AddWithDetailsAsync(PhieuNhapHang phieuNhapHang, List<ChiTietPhieuNhap> chiTietPhieuNhaps)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Log beginning of transaction
                    await _synchronizationService.LogSynchronizationAsync($"Bắt đầu giao dịch tạo phiếu nhập hàng mới");
                    
                    // Add purchase receipt
                    _context.PhieuNhapHangs.Add(phieuNhapHang);
                    await _context.SaveChangesAsync();

                    // Add purchase receipt details
                    foreach (var ct in chiTietPhieuNhaps)
                    {
                        ct.MaPhieuNhap = phieuNhapHang.MaPhieuNhap;
                        _context.ChiTietPhieuNhaps.Add(ct);
                    }
                    await _context.SaveChangesAsync();
                    
                    // Synchronize with product system
                    await _synchronizationService.SynchronizeProductsFromPurchaseReceiptAsync(phieuNhapHang, chiTietPhieuNhaps);
                    
                    // Commit transaction only if synchronization was successful
                    transaction.Commit();
                    await _synchronizationService.LogSynchronizationAsync($"Hoàn thành giao dịch tạo phiếu nhập hàng ID: {phieuNhapHang.MaPhieuNhap}");
                }
                catch (System.Exception ex)
                {
                    await _synchronizationService.LogSynchronizationAsync($"Lỗi khi tạo phiếu nhập hàng: {ex.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}