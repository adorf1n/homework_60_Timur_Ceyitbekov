using Microsoft.AspNetCore.Identity;

public class MyTask
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Priority { get; set; }
    public string Description { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? AssigneeId { get; set; } 

    public string? Assignee { get; set; }  

    public string? CreatorId { get; set; } 
    public string? Creator { get; set; }  
}
