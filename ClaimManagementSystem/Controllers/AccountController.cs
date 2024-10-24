using ClaimManagementSystem.Models; // Ensure this matches your namespace
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View("RegisterNew"); // Ensure this matches your view name
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterNew model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home"); // Redirect after successful registration
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View("RegisterNew", model); // Return to the same view if there are validation errors
    }

    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View("LoginNew"); // Use your custom LoginNew view
    }

    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl); // Redirect to the return URL or default page
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View("LoginNew", model); // Return view with validation errors
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home"); // Default redirect if not local URL
        }
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home"); // Redirect after logout
    }

    public class ClaimsController : Controller
    {
        // GET: Claims/CreateClaim
        [HttpGet]
        public IActionResult CreateClaim()
        {
            return View(); // This will look for CreateClaim.cshtml in Views/Claims
        }

        // POST: Claims/CreateClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateClaim(ClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the claim submission (e.g., save to database)
                // Redirect or return a success message
                return RedirectToAction("Index"); // Redirect to another action after successful submission
            }
            return View(model); // Return the same view with validation errors
        }
    }
}