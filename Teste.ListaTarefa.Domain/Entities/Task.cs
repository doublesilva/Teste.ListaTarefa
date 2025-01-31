namespace Teste.ListaTarefa.Domain.Entities
{
    public class Task : BaseEntity
    {
        public Task(string title, string description, int authorId = 0)
        {
            SetTitle(title);
            SetDescription(description);
            Status = TaskStatus.NotStarted;
            AuthorId = authorId;
            Comments = new List<Comment>();
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public TaskStatus Status { get; private set; }
        public int AuthorId { get; private set; }
        public virtual User Author { get; set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? DueDate { get; private set; }
        public int? OwnerId { get; private set; }
        public virtual User Owner { get; set; }
        public ICollection<Comment> Comments { get; private set; }

        /// <summary>
        /// Sets the basic information for the task.
        /// </summary>
        /// <param name="ownerId">ID of the task owner.</param>
        /// <param name="startDate">Start date of the task.</param>
        /// <param name="dueDate">Due date of the task.</param>
        public void SetInfo(int? ownerId, DateTime? startDate, DateTime? dueDate)
        {
            if (ownerId.HasValue)
                OwnerId = ownerId.Value;
            if (startDate.HasValue)
                StartDate = startDate.Value;
            if (dueDate.HasValue)
                DueDate = dueDate;
        }

        /// <summary>
        /// Starts the task and sets its status to In Progress.
        /// </summary>
        /// <param name="ownerId">ID of the task owner.</param>
        public void StartTask(int ownerId)
        {
            OwnerId = ownerId;
            StartDate = DateTime.UtcNow;
            Status = TaskStatus.InProgress;
        }

        /// <summary>
        /// Finishes the task and sets its status to Completed.
        /// </summary>
        public void FinishTask()
        {
            DueDate = DateTime.UtcNow;
            Status = TaskStatus.Completed;
        }

        /// <summary>
        /// Sets the title of the task.
        /// </summary>
        /// <param name="title">Title of the task.</param>
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty", nameof(title));
            }
            if (Title != title)
                Title = title;
        }

        /// <summary>
        /// Sets the description of the task.
        /// </summary>
        /// <param name="description">Description of the task.</param>
        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty", nameof(description));
            }
            if (Description != description)
                Description = description;
        }
    }

}
