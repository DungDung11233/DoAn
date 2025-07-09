using DoAnCoSo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Controllers
{
    [Authorize]
    public class StaffInfoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SeafoodDbContext _context;

        public StaffInfoController(UserManager<ApplicationUser> userManager, SeafoodDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult AdditionalInfo()
        {
            return View(new StaffAdditionalInfo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdditionalInfo(StaffAdditionalInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var nhanVien = new NhanVien
                    {
                        TenNhanVien = model.TenNhanVien,
                        SoDienThoai = model.SoDienThoai,
                        Email = model.Email,
                        DiaChi = model.DiaChi,
                        NgayTuyenDung = model.NgayTuyenDung,
                        UserID = user.Id
                    };

                    _context.NhanViens.Add(nhanVien);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
    }
} 