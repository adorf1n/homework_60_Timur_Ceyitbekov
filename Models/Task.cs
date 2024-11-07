using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("Tasks")]
public class MyTask
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Priority { get; set; } 

    [Required]
    public string Status { get; set; } = "Open"; 

    public string Description { get; set; }

    [Required]
    public string Assignee { get; set; }

    public DateTime CreatedAt { get; set; }
}