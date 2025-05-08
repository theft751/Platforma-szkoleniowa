namespace Domain.Models
{
    public class UserAnswerOnQuestion
    {
        public Guid Id { get; set; }
        public User User { get; set; } = null!;
        public Guid UserId { get; set; }
        public Question Question { get; set; } = null!;
        public Guid QuestionId { get; set; }
        public string? userAnswer { get; set; }
    }
}
