using Microsoft.AspNetCore.Mvc;
using FriendsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class PlanController : Controller
{
    private readonly ApplicationDbContext _context;

    public PlanController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Plan/Create
    public IActionResult Create()
    {
        int userId = int.Parse(HttpContext.Session.GetString("UserId"));

        ViewBag.Friends = _context.Friends
            .Where(f => f.UserId == userId)
            .Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            })
            .ToList();

        return View();
    }

    // POST: Plan/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Plan plan, int[] selectedFriends)
    {
        try
        {
            plan.UserId = int.Parse(HttpContext.Session.GetString("UserId"));

            // Associate selected friends with the plan
            plan.Friends = _context.Friends
                .Where(f => selectedFriends.Contains(f.Id))
                .ToList();

            _context.Plans.Add(plan);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Plan created successfully!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating plan: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while creating the plan.";
        }

        return RedirectToAction("Dashboard", "Home");
    }

    // GET: Plan/Edit/{id}
    public IActionResult Edit(int id)
    {
        var plan = _context.Plans.Include(p => p.Friends).FirstOrDefault(p => p.Id == id);
        if (plan == null)
        {
            TempData["ErrorMessage"] = "Plan not found!";
            return RedirectToAction("Dashboard", "Home");
        }

        int userId = int.Parse(HttpContext.Session.GetString("UserId"));

        ViewBag.Friends = _context.Friends
            .Where(f => f.UserId == userId)
            .Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            })
            .ToList();

        return View(plan);
    }

    // POST: Plan/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Plan updatedPlan, int[] selectedFriends)
    {
        var plan = _context.Plans.Include(p => p.Friends).FirstOrDefault(p => p.Id == updatedPlan.Id);
        if (plan == null)
        {
            TempData["ErrorMessage"] = "Plan not found!";
            return RedirectToAction("Dashboard", "Home");
        }

        try
        {
            plan.Title = updatedPlan.Title;
            plan.Place = updatedPlan.Place;
            plan.PlanDate = updatedPlan.PlanDate;

            // Clear existing friends and re-associate new ones
            plan.Friends.Clear();
            if (selectedFriends != null && selectedFriends.Any())
            {
                plan.Friends = _context.Friends
                    .Where(f => selectedFriends.Contains(f.Id))
                    .ToList();
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Plan updated successfully!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating plan: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while updating the plan.";
        }

        return RedirectToAction("Dashboard", "Home");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try
        {
            // Load the plan with its related friends
            var plan = _context.Plans.Include(p => p.Friends).FirstOrDefault(p => p.Id == id);
            if (plan != null)
            {
                // Clear the relationships with friends
                plan.Friends.Clear();

                // Save changes to update the database
                _context.SaveChanges();

                // Remove the plan itself
                _context.Plans.Remove(plan);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Plan deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan not found.";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting plan: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while deleting the plan.";
        }

        return RedirectToAction("Dashboard", "Home");
    }

}
