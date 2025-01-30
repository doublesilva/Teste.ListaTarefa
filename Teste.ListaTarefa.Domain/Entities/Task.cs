namespace Teste.ListaTarefa.Domain.Entities
{
    public class Task : BaseEntity
    {
        public Task(string title, string description, int authorId = 0)
        {
            Title = title;
            Description = description;
            Status = TaskStatus.NotStarted;
            AuthorId = authorId;
        }
        

        public string Title { get; private set; }
        public string Description { get; private set; }
        public TaskStatus Status { get; private set; }
        public int AuthorId { get; private set; }
        public virtual User Author { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? DueDate { get; private set; }
        public int? OwnerId { get; private set; }
        public virtual User Owner { get; private set; }

        public ICollection<Comment> Comments { get; set; } = [];

        public void SetInfo(string title, string description, int? ownerId, DateTime? startDate, DateTime? dueDate)
        {
            Title = title;
            Description = description;
            OwnerId = ownerId;
            StartDate = startDate;
            DueDate = dueDate;
        }

        public void StartTask(int ownerId)
        {
            OwnerId = ownerId;
            StartDate = DateTime.UtcNow;
            Status = TaskStatus.InProgress;
        }

        public void FinishTask()
        {
            DueDate = DateTime.UtcNow;
            Status = TaskStatus.Completed;
        }
    }
}
