using FluentValidation;

namespace Teste.ListaTarefa.Application.TaskApplication.Dtos
{
    public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
    {
        public TaskUpdateDtoValidator()
        {
            RuleFor(t => t.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(t => t.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(t => t.DueDate)
                .GreaterThan(t => t.StartDate)
                .When(t => t.DueDate.HasValue)
                .WithMessage("DueDate must be greater than StartDate.");
        }
    }
}
