using MediatR;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Teste.ListaTarefa.Domain.Entities;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record UpdateTaskCommand(int TaskId, TaskUpdateDto Dto) : IRequest<bool>;

    public class UpdateTaskCommandHandler(IRepository<Task> repository) : IRequestHandler<UpdateTaskCommand, bool>
    {
        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await repository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }
            var dto = request.Dto;
            task.SetInfo(dto.Title, dto.Description, dto.OwnerId, dto.StartDate, dto.DueDate);

            await repository.UpdateAsync(task, cancellationToken);

            return true;
        }
    }
}


