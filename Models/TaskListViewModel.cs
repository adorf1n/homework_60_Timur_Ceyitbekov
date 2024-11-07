public class TaskListViewModel
{
    public List<MyTask> Tasks { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string SearchTitle { get; set; }
    public string SearchPriority { get; set; }
    public string SortColumn { get; set; }  
    public string SortOrder { get; set; }   
}
