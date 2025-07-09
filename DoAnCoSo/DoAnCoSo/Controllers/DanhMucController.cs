using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using DoAnCoSo.Repositories;

namespace DoAnCoSo.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly ISanPhamRepository _sanPhamRepository;
        private readonly IDanhMucRepository _danhMucRepository;

        public DanhMucController(ISanPhamRepository sanPhamRepository, IDanhMucRepository danhMucRepository)
        {
            _sanPhamRepository = sanPhamRepository;
            _danhMucRepository = danhMucRepository;
        }

        public async Task<IActionResult> Index()
        {
            var danhMucs = await _danhMucRepository.GetAllAsync();
            return View(danhMucs);
        }

        public async Task<IActionResult> Display(int id)
        {
            var danhMuc = await _danhMucRepository.GetByIdAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                await _danhMucRepository.AddAsync(danhMuc);
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var danhMuc = await _danhMucRepository.GetByIdAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, DanhMuc danhMuc)
        {
            if (id != danhMuc.MaDanhMuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _danhMucRepository.UpdateAsync(danhMuc);
                return RedirectToAction(nameof(Index));
            }

            return View(danhMuc);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var danhMuc = await _danhMucRepository.GetByIdAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMuc = await _danhMucRepository.GetByIdAsync(id);
            if (danhMuc != null)
            {
                await _danhMucRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var danhMucs = await _danhMucRepository.GetAllAsync();
            return Json(danhMucs);
        }
    }
}
