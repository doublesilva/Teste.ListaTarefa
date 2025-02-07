using MediatR;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record TaskQuery(int TaskId) : IRequest<TaskQueryDto>;

    public class TaskQuerHandler(IRepository<Task> taskRepo) : IRequestHandler<TaskQuery, TaskQueryDto>
    {
        public async Task<TaskQueryDto> Handle(TaskQuery request, CancellationToken cancellationToken)
        {
            var task = await taskRepo.GetByIdAsync(request.TaskId, cancellationToken, x => x.Author, x => x.Owner);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }
            return task;
        }
    }
}
