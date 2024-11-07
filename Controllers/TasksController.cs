using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TasksController : Controller
{
    private readonly ToDoListContext _context;

    public TasksController(ToDoListContext context)
    {
        _context = context;
    }

    public IActionResult Index(string searchTitle, string searchPriority)
    {
        Console.WriteLine("searchTitle: " + searchTitle);
        Console.WriteLine("searchPriority: " + searchPriority);

        var tasksQuery = _context.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(searchTitle))
        {
            tasksQuery = tasksQuery.Where(t => t.Title.Contains(searchTitle));
        }

        if (!string.IsNullOrEmpty(searchPriority))
        {
            tasksQuery = tasksQuery.Where(t => t.Priority == searchPriority);
        }

        var sqlQuery = tasksQuery.ToQueryString();
        Console.WriteLine("SQL Query: " + sqlQuery);  

        var tasks = tasksQuery.ToList();

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