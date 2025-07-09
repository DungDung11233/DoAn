using System;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhuongTienVanChuyenController : Controller
    {
        private readonly IPhuongTienVanChuyenRepository _phuongTienRepository;

        public PhuongTienVanChuyenController(IPhuongTienVanChuyenRepository phuongTienRepository)
        {
            _phuongTienRepository = phuongTienRepository;
        }

        // GET: PhuongTienVanChuyen
        public async Task<IActionResult> Index()
        {
            var phuongTiens = await _phuongTienRepository.GetAllAsync();
            return View(phuongTiens);
        }

        // GET: PhuongTienVanChuyen/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var phuongTien = await _phuongTienRepository.GetByIdAsync(id);
            if (phuongTien == null)
            {
                return NotFound();
            }

            return View(phuongTien);
        }

        // GET: PhuongTienVanChuyen/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhuongTienVanChuyen/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PhuongTienVanChuyen phuongTien)
        {
            if (ModelState.IsValid)
            {
                await _phuongTienRepository.AddAsync(phuongTien);
                TempData["Success"] = "Đã thêm phương tiện vận chuyển thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(phuongTien);
        }

        // GET: PhuongTienVanChuyen/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var phuongTien = await _phuongTienRepository.GetByIdAsync(id);
            if (phuongTien == null)
            {
                return NotFound();
            }
            return View(phuongTien);
        }

        // POST: PhuongTienVanChuyen/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, PhuongTienVanChuyen phuongTien)
        {
            if (id != phuongTien.MaPhuongTien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _phuongTienRepository.UpdateAsync(phuongTien);
                    TempData["Success"] = "Đã cập nhật phương tiện vận chuyển thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await PhuongTienExists(phuongTien.MaPhuongTien);
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(phuongTien);
        }

        // GET: PhuongTienVanChuyen/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var phuongTien = await _phuongTienRepository.GetByIdAsync(id);
            if (phuongTien == null)
            {
                return NotFound();
            }

            return View(phuongTien);
        }

        // POST: PhuongTienVanChuyen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _phuongTienRepository.DeleteAsync(id);
            TempData["Success"] = "Đã xóa phương tiện vận chuyển thành công!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PhuongTienExists(int id)
        {
            var phuongTien = await _phuongTienRepository.GetByIdAsync(id);
            return phuongTien != null;
        }
    }
} 