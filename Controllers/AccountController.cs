using Microsoft.AspNetCore.Mvc;
using FriendsApp.Models;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            // Check if username already exists
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                TempData["ErrorMessage"] = "Username already exists. Please choose another one.";
                return View(user);
            }

            // Save user to the database
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Registration successful! You can now log in.";
            return RedirectToAction("Login");
        }

        TempData["ErrorMessage"] = "Registration failed. Please check your input.";
        return View(user);
    }

    // GET: Account/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            // Store UserId in the session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            TempData["SuccessMessage"] = $"Welcome back, {user.Username}!";
            return RedirectToAction("Dashboard", "Home");
        }

        TempData["ErrorMessage"] = "Invalid username or password. Please try again.";
        return View();
    }

    // GET: Account/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["SuccessMessage"] = "You have been logged out successfully.";
        return RedirectToAction("Login");
    }
}
