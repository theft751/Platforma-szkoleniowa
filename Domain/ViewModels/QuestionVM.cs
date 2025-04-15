namespace Domain.ViewModels
{
    public class QuestionVM
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string A { get; set; } = null!;
        public string B { get; set; } = null!;
        public string C { get; set; } = null!;
        public string D { get; set; } = null!;

        public string CorrectAnswer { get; set; } = null!;
    }
}
