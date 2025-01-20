using Microsoft.AspNetCore.Mvc;
using FriendsApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(); // Home page
    }

    public IActionResult Dashboard()
    {
        // Check if the user is logged in
        if (HttpContext.Session.GetString("UserId") == null)
        {
            TempData["ErrorMessage"] = "Please log in to access the dashboard.";
            return RedirectToAction("Login", "Account");
        }

        // Get the logged-in user's ID
        int userId;
        if (!int.TryParse(HttpContext.Session.GetString("UserId"), out userId))
        {
            TempData["ErrorMessage"] = "Invalid session. Please log in again.";
            return RedirectToAction("Login", "Account");
        }

        // Fetch user-specific data
        var friends = _context.Friends
    .Where(f => f.UserId == userId)
    .ToList();

        var meetings = _context.Meetings
            .Include(m => m.Friend)
            .Where(m => m.UserId == userId)
            .ToList();

        var plans = _context.Plans
            .Include(p => p.Friends) // Load associated Friends
            .Where(p => p.UserId == userId)
            .ToList();


        // Prepare data for the friend categories chart
        var friendCategories = friends
            .GroupBy(f => f.Category)
            .ToDictionary(group => group.Key, group => group.Count());

        // Prepare the Dashboard ViewModel
        var viewModel = new DashboardViewModel
        {
            Friends = friends,
            Meetings = meetings,
            Plans = plans,
            FriendCategories = friendCategories
        };

        return View(viewModel);
    }
}
