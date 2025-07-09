using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoAnCoSo.Repositories
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly SeafoodDbContext _context;
        private readonly ILogger<SynchronizationService> _logger;
        private readonly string _logFilePath = "log_sanpham.txt";

        public SynchronizationService(SeafoodDbContext context, ILogger<SynchronizationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SynchronizeProductsFromPurchaseReceiptAsync(PhieuNhapHang phieuNhapHang, List<ChiTietPhieuNhap> chiTietPhieuNhaps)
        {
            try
            {
                await LogSynchronizationAsync($"Bắt đầu đồng bộ phiếu nhập hàng ID: {phieuNhapHang.MaPhieuNhap} vào {DateTime.Now}");
                
                // Ensure all products are saved and updated
                foreach (var chiTiet in chiTietPhieuNhaps)
                {
                    var sanPham = await _context.SanPhams.FindAsync(chiTiet.MaSanPham);
                    if (sanPham == null)
                    {
                        await LogSynchronizationAsync($"Lỗi: Không tìm thấy sản phẩm ID: {chiTiet.MaSanPham}");
                        continue;
                    }

                    // Update product quantities
                    if (sanPham.SoLuong.HasValue)
                    {
                        sanPham.SoLuong += chiTiet.SoLuongSanPham;
                    }
                    else
                    {
                        sanPham.SoLuong = chiTiet.SoLuongSanPham;
                    }

                    // Handle possible null TenSanPham
                    string tenSanPham = sanPham.TenSanPham ?? "Unknown";
                    await LogSynchronizationAsync($"Cập nhật số lượng sản phẩm '{tenSanPham}' (ID: {sanPham.MaSanPham}) thành {sanPham.SoLuong}");
                }

                // Save all changes
                await _context.SaveChangesAsync();
                await LogSynchronizationAsync($"Hoàn thành đồng bộ phiếu nhập hàng ID: {phieuNhapHang.MaPhieuNhap} vào {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đồng bộ phiếu nhập hàng {MaPhieuNhap}", phieuNhapHang.MaPhieuNhap);
                await LogSynchronizationAsync($"Lỗi đồng bộ phiếu nhập hàng ID {phieuNhapHang.MaPhieuNhap}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProductQuantitiesAsync(List<ChiTietPhieuNhap> chiTietPhieuNhaps)
        {
            try
            {
                await LogSynchronizationAsync($"Bắt đầu cập nhật số lượng sản phẩm vào {DateTime.Now}");
                
                foreach (var chiTiet in chiTietPhieuNhaps)
                {
                    var sanPham = await _context.SanPhams
                        .FirstOrDefaultAsync(s => s.MaSanPham == chiTiet.MaSanPham);
                    
                    if (sanPham == null)
                    {
                        await LogSynchronizationAsync($"Lỗi: Không tìm thấy sản phẩm ID: {chiTiet.MaSanPham}");
                        continue;
                    }

                    // Update the quantity
                    if (sanPham.SoLuong.HasValue)
                    {
                        sanPham.SoLuong += chiTiet.SoLuongSanPham;
                    }
                    else
                    {
                        sanPham.SoLuong = chiTiet.SoLuongSanPham;
                    }

                    // Handle possible null TenSanPham
                    string tenSanPham = sanPham.TenSanPham ?? "Unknown";
                    await LogSynchronizationAsync($"Cập nhật số lượng sản phẩm '{tenSanPham}' (ID: {sanPham.MaSanPham}) thành {sanPham.SoLuong}");
                }

                await _context.SaveChangesAsync();
                await LogSynchronizationAsync($"Hoàn thành cập nhật số lượng sản phẩm vào {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật số lượng sản phẩm");
                await LogSynchronizationAsync($"Lỗi cập nhật số lượng sản phẩm: {ex.Message}");
                throw;
            }
        }

        public async Task LogSynchronizationAsync(string message)
        {
            try
            {
                _logger.LogInformation(message);
                
                // Also write to a dedicated log file
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi ghi log");
            }
        }
    }
} 