using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teste.ListaTarefa.Application.TaskApplication;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Teste.ListaTarefa.WebApi.Controllers.Bases;

namespace Teste.ListaTarefa.WebApi.Controllers
{
    [ApiController]
    public class TasksController(IMediator mediator, ILogger logger) : ControllerMiddlewareBase(mediator, logger)
    {
        [HttpGet, Produces("application/json", Type = typeof(List<TaskQueryDto>))]
        public async Task<IActionResult> Get()
                              => await Send(new TasksQuery());

        [HttpGet("{taskId}"), Produces("application/json", Type = typeof(TaskQueryDto))]
        public async Task<IActionResult> Get([FromRoute]int taskId)
                              => await Send(new TaskQuery(taskId));

        [HttpPost, Produces("application/json", Type = typeof(bool))]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto task)
                              => await Send(new CreateTaskCommand(task));

        [HttpPut("{taskId}"), Produces("application/json", Type = typeof(bool))]
        public async Task<IActionResult> Update([FromRoute]int taskId, [FromBody] TaskUpdateDto task)
                            => await Send(new UpdateTaskCommand(taskId, task));

        [HttpDelete("{taskId}"), Produces("application/json", Type = typeof(bool))]
        public async Task<IActionResult> Delete([FromRoute] TaskDeleteCommand request)
                            => await Send(request);
        

        [HttpPatch("start/{taskId}/to/{ownerId}"), Produces("application/json", Type = typeof(bool))]
        public async Task<IActionResult> Start([FromRoute] int taskId, [FromRoute] int ownerId)
                                => await Send(new StartTaskCommand(taskId, ownerId));


        [HttpPatch("finish/{taskId}"), Produces("application/json", Type = typeof(bool))]
        public async Task<IActionResult> Finish([FromRoute] int taskId, [FromRoute] int ownerId)
                                => await Send(new FinishTaskCommand(taskId));

    }
}
