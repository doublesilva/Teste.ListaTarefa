using MediatR;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record TasksQuery() : IRequest<List<TaskQueryDto>>;

    public class TasksQuerHandler(IRepository<Task> taskRepo) : IRequestHandler<TasksQuery, List<TaskQueryDto>>
    {
        public async Task<List<TaskQueryDto>> Handle(TasksQuery request, CancellationToken cancellationToken)
        {
            var task = await taskRepo.GetAllAsync(cancellationToken);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }
            return task.Select(x => (TaskQueryDto)x).ToList();
        }
    }
}
