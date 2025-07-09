using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IVanChuyenService
    {
        Task<bool> ProcessShippingOrderAsync(List<int> maDonHangs, int maPhuongTien, int maNhanVien);
        Task<bool> UpdateDeliveryStatusAsync(int maDonHang);
        Task<DateTime> CalculateDeliveryDateAsync(int maDonHang);
        Task<string> GetProductTypeAsync(int maDonHang);
    }
} 