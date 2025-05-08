namespace Domain.Models;

public class Question 
{
    public Guid Id  { get; set; }
    public Guid FilmId { get; set; }
    public Film Film { get; set; } = null!;
    public ICollection<UserAnswerOnQuestion> Answers { get; set; } = new List<UserAnswerOnQuestion>();
    public string Content { get; set; } = null!;
    public string A { get; set; } = null!;
    public string B { get; set; } = null!;
    public string C { get; set; } = null!;
    public string D { get; set; } = null!;
    public string CorrectAnswer { get; set; } = null!;
}