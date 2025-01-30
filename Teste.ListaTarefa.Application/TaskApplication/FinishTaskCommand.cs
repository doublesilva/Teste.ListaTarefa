using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record FinishTaskCommand(int TaskId) : IRequest<bool>;

    public class FinishTaskCommandHandler(IRepository<Task> taskRepo) : IRequestHandler<FinishTaskCommand, bool>
    {
        public async Task<bool> Handle(FinishTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
                throw new InvalidOperationException("Task not found!");
            task.FinishTask();
            await taskRepo.UpdateAsync(task, cancellationToken);
            return true;
        }
    }



}
