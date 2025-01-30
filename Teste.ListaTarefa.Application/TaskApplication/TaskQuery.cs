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
    public record TaskQuery(int TaskId) : IRequest<TaskQueryDto>;

    public class TaskQuerHandler(IRepository<Task> taskRepo) : IRequestHandler<TaskQuery, TaskQueryDto>
    {
        public async Task<TaskQueryDto> Handle(TaskQuery request, CancellationToken cancellationToken)
        {
            var task = await taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }
            return task;
        }
    }
}
