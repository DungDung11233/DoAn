using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public interface IPhieuNhapHangRepository
    {
        Task<IEnumerable<PhieuNhapHang>> GetAllAsync(); // Lấy tất cả phiếu nhập hàng
        Task<PhieuNhapHang> GetByIdAsync(int maPhieuNhap); // Lấy phiếu nhập hàng theo ID
        Task AddAsync(PhieuNhapHang phieuNhapHang); // Thêm phiếu nhập hàng
        Task UpdateAsync(PhieuNhapHang phieuNhapHang); // Cập nhật phiếu nhập hàng
        Task DeleteAsync(int maPhieuNhap); // Xóa phiếu nhập hàng

        // Phương thức mới
        Task<IEnumerable<PhieuNhapHang>> GetByNhaCungCapIdAsync(int maNhaCungCapId); // Lấy phiếu nhập theo ID nhà cung cấp
        Task AddChiTietAsync(ChiTietPhieuNhap chiTietPhieuNhap); // Thêm chi tiết phiếu nhập
        Task<IEnumerable<ChiTietPhieuNhap>> GetChiTietByPhieuNhapIdAsync(int maPhieuNhap); // Lấy chi tiết theo ID phiếu nhập
        Task AddWithDetailsAsync(PhieuNhapHang phieuNhapHang, List<ChiTietPhieuNhap> chiTietPhieuNhaps); // Thêm phiếu và chi tiết trong giao dịch
    }
}