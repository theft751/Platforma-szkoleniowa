using AutoMapper;
using Domain.Models;
using Domain.ViewModels;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;   

        public QuestionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuiz(Film film)
        {
            var questions = await _context.Questions.Where(q => q.FilmId == film.Id).ToListAsync();
            var questionsDTO = _mapper.Map<List<QuestionVM>>(questions);
            return View(questionsDTO);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(List<AnswerVM> questions)
        {
            var correctAnswers = 0;
            foreach (var question in questions)
            {
                var dbQuestion = await _context.Questions.FindAsync(question.Id);
                if (dbQuestion != null && question.UserAnswer != null && dbQuestion.CorrectAnswer == question.UserAnswer)
                {
                    correctAnswers++;
                }
            }
            ViewBag.CorrectAnswers = correctAnswers;
            return View("QuizResult");
        }
    }
}
