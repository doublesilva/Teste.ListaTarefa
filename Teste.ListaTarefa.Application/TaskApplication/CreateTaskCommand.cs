using MediatR;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record CreateTaskCommand(TaskCreateDto Input) : IRequest<bool>;

    public class CreateTaskCommandHandler(IRepository<Task> taskRepo) : IRequestHandler<CreateTaskCommand, bool>
    {
        public async Task<bool> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await taskRepo.AddAsync(request.Input, cancellationToken);
            return true;
        }
    }
}
