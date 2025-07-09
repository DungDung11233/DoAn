using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using System.Linq;

namespace DoAnCoSo.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ISanPhamRepository _sanPhamRepository;
        private readonly IDanhMucRepository _danhMucRepository;
        private readonly ILoaiSanPhamRepository _loaisanphamRepository;
        private readonly SeafoodDbContext _context;
        private readonly IChiTietKhoHangRepository _chiTietKhoHangRepository;

        public SanPhamController(
            ISanPhamRepository sanPhamRepository,
            IDanhMucRepository danhMucRepository,
            ILoaiSanPhamRepository loaisanphamRepository,
            SeafoodDbContext context,
            IChiTietKhoHangRepository chiTietKhoHangRepository)
        {
            _sanPhamRepository = sanPhamRepository;
            _danhMucRepository = danhMucRepository;
            _loaisanphamRepository = loaisanphamRepository;
            _context = context;
            _chiTietKhoHangRepository = chiTietKhoHangRepository;
        }

        // Các hành động hiện có (Index, Add, Display, Update, Delete, SanPhamTheoDanhMuc, SanPhamTheoLoai)

        public async Task<IActionResult> Index()
        {
            var danhSach = await _sanPhamRepository.GetAllAsync();
            foreach (var sanPham in danhSach)
            {
                // Kiểm tra xem sản phẩm có phải từ phiếu nhập không
                var isFromPurchaseOrder = await IsFromPurchaseOrder(sanPham.MaSanPham);
                
                // Sử dụng phương thức GetProductQuantity để lấy và đồng bộ số lượng
                sanPham.SoLuong = await GetProductQuantity(sanPham.MaSanPham, isFromPurchaseOrder);

                // Cập nhật tình trạng dựa trên số lượng
                sanPham.TinhTrang = GetTinhTrang(sanPham.SoLuong ?? 0);
            }
            return View(danhSach);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            var DanhMucs = await _danhMucRepository.GetAllAsync();
            ViewBag.DanhMucs = new SelectList(DanhMucs, "MaDanhMuc", "TenDanhMuc");
            var LoaiSanPhams = await _loaisanphamRepository.GetAllAsync();
            ViewBag.LoaiSanPhams = new SelectList(LoaiSanPhams, "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(SanPham sanPham, IFormFile imageUrl)
        {
            if (!ModelState.IsValid)
            {
                var DanhMucs = await _danhMucRepository.GetAllAsync();
                ViewBag.DanhMucs = new SelectList(DanhMucs, "MaDanhMuc", "TenDanhMuc");
                var LoaiSanPhams = await _loaisanphamRepository.GetAllAsync();
                ViewBag.LoaiSanPhams = new SelectList(LoaiSanPhams, "MaLoai", "TenLoai");
                return View(sanPham);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (imageUrl != null && imageUrl.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUrl.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageUrl.CopyToAsync(fileStream);
                        }

                        sanPham.ImageUrl = "/images/" + fileName;
                    }

                    // Get or create default warehouse first
                    var defaultWarehouse = await _context.NhaKhos.FirstOrDefaultAsync();
                    if (defaultWarehouse == null)
                    {
                        defaultWarehouse = new NhaKho
                        {
                            TenNhaKho = "Kho Mặc Định",
                            DiaChi = "Địa chỉ mặc định",
                            LoaiKho = "Kho chính"
                        };
                        await _context.NhaKhos.AddAsync(defaultWarehouse);
                        await _context.SaveChangesAsync();
                    }

                    // Add the product first
                    await _context.SanPhams.AddAsync(sanPham);
                    await _context.SaveChangesAsync();

                    // Since this product is added directly (not through purchase order),
                    // we automatically create a warehouse entry with the same quantity
                    var chiTietKho = new ChiTietKhoHang
                    {
                        MaNhaKhoID = defaultWarehouse.MaNhaKho,
                        MaSanPhamID = sanPham.MaSanPham,
                        SoLuong = sanPham.SoLuong ?? 0,
                        NgayNhap = DateTime.Now
                    };

                    await _context.ChiTietKhoHangs.AddAsync(chiTietKho);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IActionResult> Display(int id)
        {
            var sanPham = await _context.SanPhams
                .Include(s => s.DanhGias)
                    .ThenInclude(d => d.KhachHang)
                .Include(s => s.ChiTietPhieuNhaps)
                    .ThenInclude(c => c.PhieuNhapHang)
                .Include(s => s.HinhAnhSanPhams)
                .FirstOrDefaultAsync(s => s.MaSanPham == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Kiểm tra xem sản phẩm có phải từ phiếu nhập không
            var isFromPurchaseOrder = await IsFromPurchaseOrder(sanPham.MaSanPham);
            
            // Sử dụng phương thức GetProductQuantity để lấy và đồng bộ số lượng
            sanPham.SoLuong = await GetProductQuantity(sanPham.MaSanPham, isFromPurchaseOrder);

            // Cập nhật tình trạng dựa trên số lượng
            sanPham.TinhTrang = GetTinhTrang(sanPham.SoLuong ?? 0);

            // Tính trung bình đánh giá
            if (sanPham.DanhGias != null && sanPham.DanhGias.Any())
            {
                ViewBag.AverageRating = sanPham.DanhGias.Average(d => d.XepHang);
                ViewBag.TotalReviews = sanPham.DanhGias.Count();
            }
            else
            {
                ViewBag.AverageRating = 0;
                ViewBag.TotalReviews = 0;
            }

            return View(sanPham);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var sanPham = await _sanPhamRepository.GetByIdAsync(id);
            if (sanPham == null)
                return NotFound();

            // Đồng bộ số lượng từ kho
            sanPham.SoLuong = await _context.ChiTietKhoHangs
                .Where(ct => ct.MaSanPhamID == sanPham.MaSanPham)
                .SumAsync(ct => (int?)ct.SoLuong) ?? 0;

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, SanPham sanPham, IFormFile? imageUrl)
        {
            if (id != sanPham.MaSanPham)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var DanhMucs = await _danhMucRepository.GetAllAsync();
                ViewBag.DanhMucs = new SelectList(DanhMucs, "MaDanhMuc", "TenDanhMuc");
                var LoaiSanPhams = await _loaisanphamRepository.GetAllAsync();
                ViewBag.LoaiSanPhams = new SelectList(LoaiSanPhams, "MaLoai", "TenLoai");
                return View(sanPham);
            }

            try
            {
                var existingSanPham = await _sanPhamRepository.GetByIdAsync(id);
                if (existingSanPham == null)
                {
                    return NotFound();
                }

                existingSanPham.TenSanPham = sanPham.TenSanPham;
                existingSanPham.NguonGoc = sanPham.NguonGoc;
                existingSanPham.NgayThuHoach = sanPham.NgayThuHoach;
                existingSanPham.LoaiBaoQuan = sanPham.LoaiBaoQuan;
                existingSanPham.GiaTheoKG = sanPham.GiaTheoKG;
                existingSanPham.TinhTrang = sanPham.TinhTrang;
                existingSanPham.DanhMucID = sanPham.DanhMucID;
                existingSanPham.MaLoaiID = sanPham.MaLoaiID;

                if (imageUrl != null && imageUrl.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUrl.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageUrl.CopyToAsync(fileStream);
                    }

                    if (!string.IsNullOrEmpty(existingSanPham.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingSanPham.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    existingSanPham.ImageUrl = "/images/" + fileName;
                }

                // Đồng bộ số lượng (nếu có thay đổi trực tiếp từ form)
                if (sanPham.SoLuong.HasValue)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        var currentTotal = await _context.ChiTietKhoHangs
                            .Where(ct => ct.MaSanPhamID == id)
                            .SumAsync(ct => (int?)ct.SoLuong) ?? 0;
                        var difference = sanPham.SoLuong.Value - currentTotal;
                        existingSanPham.SoLuong = sanPham.SoLuong.Value;

                        // Cập nhật số lượng trong kho
                        if (difference != 0)
                        {
                            // Lấy kho mặc định
                            var defaultWarehouse = await _context.NhaKhos.FirstOrDefaultAsync();
                            if (defaultWarehouse == null)
                            {
                                throw new Exception("Không tìm thấy kho mặc định");
                            }

                            // Tìm chi tiết kho của sản phẩm
                            var chiTietKho = await _context.ChiTietKhoHangs
                                .FirstOrDefaultAsync(ct => ct.MaNhaKhoID == defaultWarehouse.MaNhaKho && ct.MaSanPhamID == id);

                            if (chiTietKho != null)
                            {
                                // Cập nhật số lượng trong kho
                                chiTietKho.SoLuong += difference;
                                _context.Update(chiTietKho);
                            }
                            else
                            {
                                // Tạo mới chi tiết kho nếu chưa tồn tại
                                chiTietKho = new ChiTietKhoHang
                                {
                                    MaNhaKhoID = defaultWarehouse.MaNhaKho,
                                    MaSanPhamID = id,
                                    SoLuong = sanPham.SoLuong.Value,
                                    NgayNhap = DateTime.Now
                                };
                                await _context.ChiTietKhoHangs.AddAsync(chiTietKho);
                            }
                        }

                        await _sanPhamRepository.UpdateAsync(existingSanPham);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sản phẩm: " + ex.Message);
                        return View(sanPham);
                    }
                }

                await _sanPhamRepository.UpdateAsync(existingSanPham);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sản phẩm: " + ex.Message);
                return View(sanPham);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var sanPham = await _sanPhamRepository.GetByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            return View(sanPham);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var sanPham = await _context.SanPhams
                    .Include(s => s.ChiTietKhoHangs)
                    .FirstOrDefaultAsync(s => s.MaSanPham == id);

                if (sanPham == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Xóa tất cả các chi tiết kho hàng liên quan đến sản phẩm
                        if (sanPham.ChiTietKhoHangs != null && sanPham.ChiTietKhoHangs.Any())
                        {
                            _context.ChiTietKhoHangs.RemoveRange(sanPham.ChiTietKhoHangs);
                        }

                        // Xóa sản phẩm
                        _context.SanPhams.Remove(sanPham);
                        
                        // Lưu các thay đổi
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        
                        TempData["SuccessMessage"] = "Xóa sản phẩm và thông tin kho thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi xóa sản phẩm: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Kiểm tra MaSanPham và chuyển hướng đến ManageHinhAnh
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckMaSanPham(int maSanPham)
        {
            var sanPham = await _context.SanPhams.FindAsync(maSanPham);
            if (sanPham == null)
            {
                TempData["ErrorMessage"] = $"Mã sản phẩm {maSanPham} không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("ManageHinhAnh", new { maSanPham });
        }

        // Quản lý HinhAnhSanPham
        [Authorize(Roles = "Admin,Nhà cung cấp")]
        public async Task<IActionResult> ManageHinhAnh(int maSanPham)
        {
            var sanPham = await _context.SanPhams
                .Include(sp => sp.HinhAnhSanPhams)
                .FirstOrDefaultAsync(sp => sp.MaSanPham == maSanPham);
            if (sanPham == null)
            {
                TempData["ErrorMessage"] = $"Mã sản phẩm {maSanPham} không tồn tại.";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MaSanPham = maSanPham;
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Nhà cung cấp")]
        public async Task<IActionResult> AddHinhAnh(int maSanPham, IFormFile imageFile)
        {
            var sanPham = await _context.SanPhams.FindAsync(maSanPham);
            if (sanPham == null || imageFile == null || imageFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm ảnh phụ.";
                return RedirectToAction("ManageHinhAnh", new { maSanPham });
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            var hinhAnh = new HinhAnhSanPham
            {
                LoaiHinhAnh = "/images/" + fileName,
                MaSanPhamID = maSanPham
            };
            _context.HinhAnhSanPhams.Add(hinhAnh);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageHinhAnh", new { maSanPham });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Nhà cung cấp")]
        public async Task<IActionResult> EditHinhAnh(int maHinhAnh, IFormFile imageFile)
        {
            var hinhAnh = await _context.HinhAnhSanPhams.FindAsync(maHinhAnh);
            if (hinhAnh == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa ảnh phụ.";
                return RedirectToAction("Index", "Home");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (imageFile != null && imageFile.Length > 0)
            {
                // Xóa ảnh cũ
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hinhAnh.LoaiHinhAnh.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                // Tải lên ảnh mới
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                hinhAnh.LoaiHinhAnh = "/images/" + fileName;
            }

            _context.HinhAnhSanPhams.Update(hinhAnh);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageHinhAnh", new { maSanPham = hinhAnh.MaSanPhamID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Nhà cung cấp")]
        public async Task<IActionResult> DeleteHinhAnh(int maHinhAnh)
        {
            var hinhAnh = await _context.HinhAnhSanPhams.FindAsync(maHinhAnh);
            if (hinhAnh == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa ảnh phụ.";
                return RedirectToAction("Index", "Home");
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hinhAnh.LoaiHinhAnh.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.HinhAnhSanPhams.Remove(hinhAnh);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageHinhAnh", new { maSanPham = hinhAnh.MaSanPhamID });
        }
        public async Task<IActionResult> SanPhamTheoDanhMuc(int id)
        {
            var sanPhams = await _sanPhamRepository.GetAllAsync();
            var filteredSanPhams = sanPhams.Where(sp => sp.DanhMucID == id).ToList();

            // Lấy tên danh mục để hiển thị
            var danhMuc = await _danhMucRepository.GetByIdAsync(id);
            ViewBag.TenDanhMuc = danhMuc?.TenDanhMuc ?? "Danh mục không xác định";

            return View(filteredSanPhams);
        }
        private string GetTinhTrang(int soLuong)
        {
            if (soLuong > 10) return "In Stock";
            if (soLuong > 0) return "Low Stock";
            return "Out of Stock";
        }

        [HttpPost]
        [Authorize] // Yêu cầu đăng nhập để đánh giá
        [Authorize(Roles = "Admin,Khách hàng")]
        public async Task<IActionResult> AddReview(int maSanPham, int xepHang, string binhLuan)
        {
            if (xepHang < 1 || xepHang > 5)
            {
                return BadRequest("Xếp hạng phải từ 1 đến 5 sao");
            }

            var sanPham = await _sanPhamRepository.GetByIdAsync(maSanPham);
            if (sanPham == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nguoiDung = await _context.KhachHangs.FirstOrDefaultAsync(n => n.UserID == userId);

            if (nguoiDung == null)
            {
                return BadRequest("Không tìm thấy thông tin người dùng");
            }

            var danhGia = new DanhGia
            {
                MaSanPhamID = maSanPham,
                MaKhachHangID = nguoiDung.MaKhachHang,
                XepHang = xepHang,
                BinhLuan = binhLuan,
                NgayDanhGia = DateTime.Now
            };

            await _context.DanhGias.AddAsync(danhGia);
            await _context.SaveChangesAsync();

            return RedirectToAction("Display", new { id = maSanPham });
        }

        // Thêm phương thức mới để lấy số lượng sản phẩm
        private async Task<int> GetProductQuantity(int maSanPham, bool isFromPurchaseOrder)
        {
            // Lấy tổng số lượng từ kho (bao gồm cả sản phẩm từ phiếu nhập và nhập trực tiếp)
            var soLuongTrongKho = await _context.ChiTietKhoHangs
                .Where(ct => ct.MaSanPhamID == maSanPham)
                .SumAsync(ct => (int?)ct.SoLuong) ?? 0;

            // Cập nhật số lượng trong bảng SanPham để đồng bộ
            var sanPham = await _context.SanPhams.FindAsync(maSanPham);
            if (sanPham != null && sanPham.SoLuong != soLuongTrongKho)
            {
                sanPham.SoLuong = soLuongTrongKho;
                await _context.SaveChangesAsync();
            }

            return soLuongTrongKho;
        }

        private async Task<bool> IsFromPurchaseOrder(int maSanPham)
        {
            return await _context.ChiTietPhieuNhaps
                .AnyAsync(ct => ct.MaSanPham == maSanPham);
        }

        [HttpGet]
        [Route("GetQuantity/{id}")]
        public async Task<IActionResult> GetQuantity(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            var isFromPurchaseOrder = await IsFromPurchaseOrder(id);
            var quantity = await GetProductQuantity(id, isFromPurchaseOrder);

            return Json(quantity);
        }
    }
}