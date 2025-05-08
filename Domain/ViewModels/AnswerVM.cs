namespace Domain.ViewModels
{
    public class AnswerVM
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? UserAnswer { get; set; }
    }
}
