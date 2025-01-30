using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Teste.ListaTarefa.Domain.Entities;
using Teste.ListaTarefa.Domain.Interfaces;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.Application.TaskApplication
{
    public record TaskDeleteCommand(int TaskId) : IRequest<bool>;

    public class TaskDeleteCommandHandler(IRepository<Task> taskRepo) : IRequestHandler<TaskDeleteCommand, bool>
    {
        public async Task<bool> Handle(TaskDeleteCommand request, CancellationToken cancellationToken)
        {            
            await taskRepo.DeleteAsync(request.TaskId, cancellationToken);
            return true;
        }
    }
}
