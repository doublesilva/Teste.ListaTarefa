namespace Teste.ListaTarefa.Application.TaskApplication.Dtos
{
    public record TaskUpdateDto(string Title, string Description, DateTime? StartDate, DateTime? DueDate, int? OwnerId)
    {
    }


}
