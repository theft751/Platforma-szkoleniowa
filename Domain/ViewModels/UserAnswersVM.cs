namespace Domain.ViewModels
{
    public class UserAnswersVM
    {
        public string UserName { get; set; } = null!;
        public IList<AnswerVM> Answers { get; set; } = new List<AnswerVM>();
    }
}
