using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste.ListaTarefa.Application.TaskApplication.Dtos
{
    public class TaskCreateDtoValidator : AbstractValidator<TaskCreateDto>
    {
        public TaskCreateDtoValidator()
        {
            RuleFor(t => t.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(t => t.Description).NotEmpty().WithMessage("Description is required.");
        }
    }
}
