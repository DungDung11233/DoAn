using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DoAnCoSo.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        private readonly ILoaiSanPhamRepository _loaiSanPhamRepository;

        public LoaiSanPhamController(ILoaiSanPhamRepository loaiSanPhamRepository)
        {
            _loaiSanPhamRepository = loaiSanPhamRepository;
        }

        public async Task<IActionResult> Index()
        {
            var danhSach = await _loaiSanPhamRepository.GetAllAsync();
            return View(danhSach);
        }

        public async Task<IActionResult> Display(int id)
        {
            var loaiSanPham = await _loaiSanPhamRepository.GetByIdAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(LoaiSanPham loaiSanPham)
        {
            if (!ModelState.IsValid)
            {
                return View(loaiSanPham);
            }

            await _loaiSanPhamRepository.AddAsync(loaiSanPham);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var loaiSanPham = await _loaiSanPhamRepository.GetByIdAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.MaLoai)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(loaiSanPham);
            }

            try
            {
                var existingLoaiSanPham = await _loaiSanPhamRepository.GetByIdAsync(id);
                if (existingLoaiSanPham == null)
                {
                    return NotFound();
                }

                existingLoaiSanPham.TenLoai = loaiSanPham.TenLoai;
                existingLoaiSanPham.GhiChu = loaiSanPham.GhiChu;

                await _loaiSanPhamRepository.UpdateAsync(existingLoaiSanPham);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật loại sản phẩm: " + ex.Message);
                return View(loaiSanPham);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var loaiSanPham = await _loaiSanPhamRepository.GetByIdAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var loaiSanPham = await _loaiSanPhamRepository.GetByIdAsync(id);
                if (loaiSanPham == null)
                {
                    return NotFound();
                }

                await _loaiSanPhamRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi xóa loại sản phẩm: " + ex.Message);
                return View();
            }
        }
    }
}
