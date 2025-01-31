using MediatR;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record CreateTaskCommand(TaskCreateDto Input) : IRequest<int>;

    public class CreateTaskCommandHandler(IRepository<Task> taskRepo) : IRequestHandler<CreateTaskCommand, int>
    {
        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = (Task)request.Input;
            await taskRepo.AddAsync(task, cancellationToken);
            return task.Id;
        }
    }
}
