using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

[Authorize]
public class TasksController : Controller
{
    private readonly ToDoListContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TasksController(ToDoListContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string searchTitle, string searchPriority, string sortOrder)
    {
        var tasksQuery = _context.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(searchTitle))
        {
            tasksQuery = tasksQuery.Where(t => t.Title.Contains(searchTitle));
        }

        if (!string.IsNullOrEmpty(searchPriority))
        {
            tasksQuery = tasksQuery.Where(t => t.Priority == searchPriority);
        }

        switch (sortOrder)
        {
            case "Title_asc":
                tasksQuery = tasksQuery.OrderBy(t => t.Title);
                break;
            case "Title_desc":
                tasksQuery = tasksQuery.OrderByDescending(t => t.Title);
                break;
            case "Priority_asc":
                tasksQuery = tasksQuery.OrderBy(t => t.Priority);
                break;
            case "Priority_desc":
                tasksQuery = tasksQuery.OrderByDescending(t => t.Priority);
                break;
            case "Status_asc":
                tasksQuery = tasksQuery.OrderBy(t => t.Status);
                break;
            case "Status_desc":
                tasksQuery = tasksQuery.OrderByDescending(t => t.Status);
                break;
            case "CreatedAt_asc":
                tasksQuery = tasksQuery.OrderBy(t => t.CreatedAt);
                break;
            case "CreatedAt_desc":
                tasksQuery = tasksQuery.OrderByDescending(t => t.CreatedAt);
                break;
            default:
                tasksQuery = tasksQuery.OrderBy(t => t.Title);
                break;
        }

        var tasks = await tasksQuery.ToListAsync();

        ViewData["TitleSortOrder"] = sortOrder == "Title_asc" ? "Title_desc" : "Title_asc";
        ViewData["PrioritySortOrder"] = sortOrder == "Priority_asc" ? "Priority_desc" : "Priority_asc";
        ViewData["StatusSortOrder"] = sortOrder == "Status_asc" ? "Status_desc" : "Status_asc";
        ViewData["CreatedAtSortOrder"] = sortOrder == "CreatedAt_asc" ? "CreatedAt_desc" : "CreatedAt_asc";

        return View(tasks);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Priority,Description")] MyTask myTask)
    {

        if (User.Identity?.IsAuthenticated == true)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserName = User.Identity.Name;

            Console.WriteLine("Creating task...");
            Console.WriteLine($"Title: {myTask.Title}");
            Console.WriteLine($"Priority: {myTask.Priority}");
            Console.WriteLine($"Description: {myTask.Description}");
            Console.WriteLine($"Current User ID: {currentUserId}");
            Console.WriteLine($"Assigned User (AssigneeId): {currentUserId}");
            Console.WriteLine($"Creator User (CreatorId): {currentUserId}");

            myTask.AssigneeId = currentUserId;
            myTask.Assignee = currentUserName;
            myTask.CreatorId = currentUserId;
            myTask.Creator = currentUserName;
            myTask.CreatedAt = DateTime.UtcNow;
            myTask.Status = "Open";

            if (ModelState.IsValid)
            {
                _context.Add(myTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }
        }

        return View(myTask);
    }





    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var myTask = await _context.Tasks
            .FirstOrDefaultAsync(m => m.Id == id);

        if (myTask == null)
        {
            return NotFound();
        }

        return View(myTask);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        var currentUserId = _userManager.GetUserId(User); 

        if (task.AssigneeId != null) 
        {
            return Forbid(); 
        }

        task.AssigneeId = currentUserId;
        task.Status = "In Progress";  
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Close(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        task.Status = "Closed"; 
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null || task.Status == "Open")
        {
            return Forbid(); 
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
