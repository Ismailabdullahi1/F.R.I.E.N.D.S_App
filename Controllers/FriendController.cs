using Microsoft.AspNetCore.Mvc;
using FriendsApp.Models;
using Microsoft.EntityFrameworkCore;

public class FriendController : Controller
{
    private readonly ApplicationDbContext _context;

    public FriendController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Friend/Create
    public IActionResult Create()
    {
        return View(); // Render the Create Friend form
    }

    // POST: Friend/Create
    [HttpPost]
    public IActionResult Create(Friend friend)
    {
        // Get the logged-in user's ID from the session
        var sessionUserId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(sessionUserId) || !int.TryParse(sessionUserId, out int userId))
        {
            TempData["ErrorMessage"] = "Session expired. Please log in again.";
            return RedirectToAction("Login", "Account");
        }

        // Assign the logged-in user's ID
        friend.UserId = userId;

        // Save the friend to the database
        _context.Friends.Add(friend);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Friend added successfully!";
        return RedirectToAction("Dashboard", "Home");
    }


    [HttpPost]
    public IActionResult Edit(Friend friend)
    {
        var existingFriend = _context.Friends.Find(friend.Id);
        if (existingFriend == null)
        {
            TempData["ErrorMessage"] = "Friend not found.";
            return RedirectToAction("Dashboard", "Home");
        }

        existingFriend.Name = friend.Name;
        existingFriend.PhoneNumber = friend.PhoneNumber;
        existingFriend.Location = friend.Location;
        existingFriend.Category = friend.Category;
        existingFriend.FirstMeetingPlace = friend.FirstMeetingPlace;

        _context.Friends.Update(existingFriend);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Friend updated successfully!";
        return RedirectToAction("Dashboard", "Home");
    }

    public IActionResult Delete(int id)
    {
        try
        {
            // Fetch the friend and check if it exists
            var friend = _context.Friends.Include(f => f.Meetings).Include(f => f.Plans).FirstOrDefault(f => f.Id == id);
            if (friend == null)
            {
                TempData["ErrorMessage"] = "Friend not found.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Remove associated meetings
            var associatedMeetings = _context.Meetings.Where(m => m.FriendId == id).ToList();
            if (associatedMeetings.Any())
            {
                _context.Meetings.RemoveRange(associatedMeetings);
            }

            // Remove associated plans if applicable (many-to-many)
            var associatedPlans = _context.Plans.Include(p => p.Friends).Where(p => p.Friends.Any(f => f.Id == id)).ToList();
            foreach (var plan in associatedPlans)
            {
                plan.Friends.Remove(friend); // Remove the friend from the plan's friends list
            }

            // Remove the friend itself
            _context.Friends.Remove(friend);

            // Save changes
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Friend deleted successfully!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Log error for debugging
            TempData["ErrorMessage"] = "An error occurred while deleting the friend. Please try again.";
        }

        return RedirectToAction("Dashboard", "Home");
    }



}
