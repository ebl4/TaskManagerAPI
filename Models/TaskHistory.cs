public class TaskHistory
{
    public int Id { get; set; }
    public string FieldModified { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string ModifiedBy { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
}