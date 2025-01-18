public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; } // "Low", "Medium", "High"
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public List<TaskHistory> History { get; set; }
}