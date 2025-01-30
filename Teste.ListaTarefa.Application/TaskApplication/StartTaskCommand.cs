using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record StartTaskCommand(int TaskId, int OwnerId) : IRequest<bool>;

    public class StartTaskCommandHandler(IRepository<Task> taskRepo) : IRequestHandler<StartTaskCommand, bool>
    {
        public async Task<bool> Handle(StartTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
                throw new InvalidOperationException("Task not found!");
            task.StartTask(request.OwnerId);
            await taskRepo.UpdateAsync(task, cancellationToken);
            return true;
        }
    }



}
