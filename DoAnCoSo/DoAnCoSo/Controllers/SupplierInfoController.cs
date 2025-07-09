using DoAnCoSo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCoSo.Controllers
{
    [Authorize]
    public class SupplierInfoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SeafoodDbContext _context;

        public SupplierInfoController(UserManager<ApplicationUser> userManager, SeafoodDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult AdditionalInfo()
        {
            return View(new SupplierAdditionalInfo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdditionalInfo(SupplierAdditionalInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var nhaCungCap = new NhaCungCap
                    {
                        TenNCC = model.TenNCC,
                        SDT = model.SDT,
                        Email = model.Email,
                        DiaChiNCC = model.DiaChiNCC,
                        UserID = user.Id
                    };

                    _context.NhaCungCaps.Add(nhaCungCap);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
    }
} 