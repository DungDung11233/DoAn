using DoAnCoSo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Controllers
{
    [Authorize]
    public class CustomerInfoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SeafoodDbContext _context;

        public CustomerInfoController(UserManager<ApplicationUser> userManager, SeafoodDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult AdditionalInfo()
        {
            return View(new CustomerAdditionalInfo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdditionalInfo(CustomerAdditionalInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var nguoiDung = new KhachHang
                    {
                        TenKhachHang = model.TenNguoiDung,
                        SoDienThoai = model.SoDienThoai,
                        Email = model.Email,
                        DiaChi = model.DiaChi,  
                        NgaySinh = model.NgaySinh,
                        UserID = user.Id
                    };

                    _context.KhachHangs.Add(nguoiDung);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
    }
} 