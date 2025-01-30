using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.ListaTarefa.Domain.Entities;
using TaskStatus = Teste.ListaTarefa.Domain.Entities.TaskStatus;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication.Dtos
{
    public record TaskQueryDto(string Title, string Description, TaskStatus Status, 
                               DateTime? StartDate, DateTime? DueDate, string OwnerName, string AuthorName,
                               DateTime CreatedOn, DateTime? UpdatedOn
                      )
    {
        public static implicit operator TaskQueryDto(Task task)
            => new(task.Title, task.Description, task.Status, task.StartDate,
                task.DueDate, task.Owner?.Name, task.Author?.Name, task.CreatedOn, 
                task.UpdatedOn);
    }
}
