using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teste.ListaTarefa.Domain.Entities;

namespace Teste.ListaTarefa.WebApi.Controllers.Bases
{

    [Route("api/[controller]")]
    [ApiController]
    public class ControllerMiddlewareBase(IMediator mediator, ILogger logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger _logger = logger;

        public async Task<IActionResult> Send<TInput>(TInput input)
            where TInput : class
        {
            {
                try
                {
                    var output = await _mediator.Send(input);
                    return Ok(output);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, "Não foi possivel executar ação @{Action} {User} {Message}", input, User?.Identity?.Name, ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (ValidationException ex)
                {
                    _logger.LogError(ex, "Não foi possivel executar ação @{Action} {User} {Message}", input, User?.Identity?.Name, ex.Message);
                    return BadRequest(ex.Message);
                }
                catch (AccessViolationException ex)
                {
                    _logger.LogError(ex, "Não foi possivel executar ação @{Action} {User} {Message}", input, User?.Identity?.Name, ex.Message);
                    return Unauthorized(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Falha ao tentar executar ação {Action} {User} {Message}", input, User?.Identity?.Name, ex.Message);
                    return base.StatusCode(500, "Falha ao tentar executar ação");
                }
            }
        }


    }
}
