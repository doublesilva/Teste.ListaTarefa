using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication.Dtos
{
    public record TaskCreateDto(string Title, string Description)
    {
        public static implicit operator Task(TaskCreateDto input)
            => new Task(input.Title, input.Description);
    }
}
