using Microsoft.AspNetCore.Mvc;
using FriendsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class MeetingController : Controller
{
    private readonly ApplicationDbContext _context;

    public MeetingController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int? GetLoggedInUserId()
    {
        var sessionUserId = HttpContext.Session.GetString("UserId");
        if (!string.IsNullOrEmpty(sessionUserId) && int.TryParse(sessionUserId, out int userId))
        {
            return userId;
        }

        return null;
    }

    // GET: Meeting/Create
    public IActionResult Create()
    {
        var userId = GetLoggedInUserId();
        if (userId == null)
        {
            TempData["ErrorMessage"] = "Session expired. Please log in again.";
            return RedirectToAction("Login", "Account");
        }

        ViewBag.Friends = _context.Friends
            .Where(f => f.UserId == userId.Value)
            .Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList();

        return View();
    }

    // POST: Meeting/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Meeting meeting)
    {
        var userId = GetLoggedInUserId();
        if (userId == null)
        {
            TempData["ErrorMessage"] = "Session expired. Please log in again.";
            return RedirectToAction("Login", "Account");
        }

        meeting.UserId = userId.Value;

        try
        {
            _context.Meetings.Add(meeting);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Meeting created successfully!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating meeting: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while creating the meeting.";
        }

        return RedirectToAction("Dashboard", "Home");
    }

    // GET: Meeting/Edit/{id}
    public IActionResult Edit(int id)
    {
        var userId = GetLoggedInUserId();
        if (userId == null)
        {
            TempData["ErrorMessage"] = "Session expired. Please log in again.";
            return RedirectToAction("Login", "Account");
        }

        var meeting = _context.Meetings.FirstOrDefault(m => m.Id == id && m.UserId == userId.Value);
        if (meeting == null)
        {
            TempData["ErrorMessage"] = "Meeting not found or you do not have access.";
            return RedirectToAction("Dashboard", "Home");
        }

        ViewBag.Friends = _context.Friends
            .Where(f => f.UserId == userId.Value)
            .Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList();

        return View(meeting);
    }

    // POST: Meeting/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Meeting meeting)
    {
        var userId = GetLoggedInUserId();
        if (userId == null)
        {
            TempData["ErrorMessage"] = "Session expired. Please log in again.";
            return RedirectToAction("Login", "Account");
        }

        var existingMeeting = _context.Meetings.FirstOrDefault(m => m.Id == meeting.Id && m.UserId == userId.Value);
        if (existingMeeting == null)
        {
            TempData["ErrorMessage"] = "Meeting not found or you do not have access.";
            return RedirectToAction("Dashboard", "Home");
        }

        try
        {
            existingMeeting.MeetingDate = meeting.MeetingDate;
            existingMeeting.MeetingTime = meeting.MeetingTime;
            existingMeeting.Place = meeting.Place;
            existingMeeting.FriendId = meeting.FriendId;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Meeting updated successfully!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating meeting: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while updating the meeting.";
        }

        return RedirectToAction("Dashboard", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try
        {
            // Directly attach and remove the entity
            var meeting = new Meeting { Id = id };
            _context.Meetings.Attach(meeting); // Attach the entity to the context
            _context.Meetings.Remove(meeting); // Mark it for removal
            _context.SaveChanges(); // Save changes to delete from the database

            TempData["SuccessMessage"] = "Meeting deleted successfully!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting meeting: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred while deleting the meeting.";
        }

        return RedirectToAction("Dashboard", "Home");
    }

}
