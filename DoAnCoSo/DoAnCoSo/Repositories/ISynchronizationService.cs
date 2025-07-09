using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface ISynchronizationService
    {
        /// <summary>
        /// Synchronizes the product inventory after purchase receipt creation
        /// </summary>
        /// <param name="phieuNhapHang">The purchase receipt</param>
        /// <param name="chiTietPhieuNhaps">The purchase receipt details</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SynchronizeProductsFromPurchaseReceiptAsync(PhieuNhapHang phieuNhapHang, List<ChiTietPhieuNhap> chiTietPhieuNhaps);
        
        /// <summary>
        /// Updates product quantities based on purchase receipt details
        /// </summary>
        /// <param name="chiTietPhieuNhaps">The purchase receipt details</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdateProductQuantitiesAsync(List<ChiTietPhieuNhap> chiTietPhieuNhaps);
        
        /// <summary>
        /// Logs the synchronization operations
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task LogSynchronizationAsync(string message);
    }
} 