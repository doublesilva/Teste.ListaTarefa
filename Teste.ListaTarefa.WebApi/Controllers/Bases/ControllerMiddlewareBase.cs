using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Teste.ListaTarefa.WebApi.Controllers.Bases
{

    [Route("api/[controller]")]
    [ApiController]
    public class ControllerMiddlewareBase(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

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
                    return BadRequest(ex.Message);
                }
                catch (ValidationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (AccessViolationException ex)
                {
                    return Unauthorized(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return base.StatusCode(500, "Falha ao tentar executar ação");
                }
            }
        }


    }
}
