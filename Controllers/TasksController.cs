using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TasksController : Controller
{
    private readonly ToDoListContext _context;

    public TasksController(ToDoListContext context)
    {
        _context = context;
    }

    [Authorize]
    public IActionResult Index(string searchTitle, string searchPriority, string sortOrder)
    {
        Console.WriteLine("searchTitle: " + searchTitle);
        Console.WriteLine("searchPriority: " + searchPriority);
        Console.WriteLine("sortOrder: " + sortOrder);

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

        var sqlQuery = tasksQuery.ToQueryString();
        Console.WriteLine("SQL Query: " + sqlQuery);

        var tasks = tasksQuery.ToList();

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
    public async Task<IActionResult> Create([Bind("Title,Priority,Description,Assignee,Status")] MyTask myTask)
    {
        if (ModelState.IsValid)
        {
            myTask.CreatedAt = DateTime.UtcNow;

            _context.Add(myTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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