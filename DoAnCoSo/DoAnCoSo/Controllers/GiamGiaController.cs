using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using DoAnCoSo.Extensions;

namespace DoAnCoSo.Controllers
{
    public class GiamGiaController : Controller
    {
        private readonly IGiamGiaRepository _giamGiaRepository;
        private readonly SeafoodDbContext _context;

        public GiamGiaController(IGiamGiaRepository giamGiaRepository, SeafoodDbContext context)
        {
            _giamGiaRepository = giamGiaRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var giamGias = await _giamGiaRepository.GetAllAsync();
            return View(giamGias);
        }

        public async Task<IActionResult> Display(int id)
        {
            var giamGia = await _giamGiaRepository.GetByIdAsync(id);
            if (giamGia == null)
            {
                return NotFound();
            }
            return View(giamGia);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(GiamGia giamGia)
        {
            if (ModelState.IsValid)
            {
                await _giamGiaRepository.AddAsync(giamGia);
                return RedirectToAction(nameof(Index));
            }
            return View(giamGia);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var giamGia = await _giamGiaRepository.GetByIdAsync(id);
            if (giamGia == null)
            {
                return NotFound();
            }
            return View(giamGia);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, GiamGia giamGia)
        {
            if (id != giamGia.MaGiamGia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _giamGiaRepository.UpdateAsync(giamGia);
                return RedirectToAction(nameof(Index));
            }

            return View(giamGia);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var giamGia = await _giamGiaRepository.GetByIdAsync(id);
            if (giamGia == null)
            {
                return NotFound();
            }
            return View(giamGia);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giamGia = await _giamGiaRepository.GetByIdAsync(id);
            if (giamGia != null)
            {
                await _giamGiaRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }    
    }
}
