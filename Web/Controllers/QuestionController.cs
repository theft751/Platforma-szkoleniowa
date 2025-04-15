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
        public async Task<IActionResult> GetQuiz(Guid filmId)
        {
            var questions = await _context.Questions.Where(q => q.FilmId == filmId).ToListAsync();

            var answers = questions.Select(q => new AnswerVM
            {
                Id = q.Id,
                Content = q.Content
            }).ToList();

            return View(answers);
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

        [HttpGet]
        public IActionResult Create(Guid filmId)
        {
            var questionVM = new QuestionVM { Id = Guid.NewGuid() };
            ViewBag.FilmId = filmId;
            return View(questionVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid filmId, QuestionVM questionVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FilmId = filmId;
                return View(questionVM);
            }

            var question = new Question
            {
                Id = Guid.NewGuid(),
                FilmId = filmId,
                Content = questionVM.Content,
                A = questionVM.A,
                B = questionVM.B,
                C = questionVM.C,
                D = questionVM.D,
                CorrectAnswer = questionVM.CorrectAnswer
            };

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Films", new { id = filmId }); // lub inna strona
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            var vm = new QuestionVM
            {
                Id = question.Id,
                Content = question.Content,
                A = question.A,
                B = question.B,
                C = question.C,
                D = question.D,
                CorrectAnswer = question.CorrectAnswer
            };

            ViewBag.FilmId = question.FilmId;
            return View(vm);
        }

        // POST: /Question/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, QuestionVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            question.Content = vm.Content;
            question.A = vm.A;
            question.B = vm.B;
            question.C = vm.C;
            question.D = vm.D;
            question.CorrectAnswer = vm.CorrectAnswer;

            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Films", new { id = question.FilmId });
        }

        // POST: /Question/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            var filmId = question.FilmId;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Films", new { id = filmId });
        }
    }
}
