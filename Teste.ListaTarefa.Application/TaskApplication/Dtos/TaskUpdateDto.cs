using System.Reflection;
using Teste.ListaTarefa.Domain.Entities;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication.Dtos
{
    public record TaskUpdateDto(string Title, string Description, DateTime? StartDate, DateTime? DueDate, int? OwnerId)
    {
    }


}
