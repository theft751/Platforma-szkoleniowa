namespace Domain.ViewModels
{
    public class AnswerVM
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string? UserAnswer { get; set; }
    }
}
