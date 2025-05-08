using AutoMapper;
using Domain.Models;
using Domain.ViewModels;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User>_userManager;

        public QuestionController(AppDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuiz(Guid filmId)
        {
            var questions = await _context.Questions.Where(q => q.FilmId == filmId).ToListAsync();

            var answers = questions.Select(q => new AnswerVM
            {
                Id = q.Id,
                Content = q.Content,
                A = q.A,
                B = q.B,
                C = q.C,
                D = q.D
            }).ToList();

            return View(answers);
        }

        [HttpPost]
        //public async Task<IActionResult> SubmitQuiz(List<AnswerVM> questions)
        //{
        //    var correctAnswers = 0;
        //    foreach (var question in questions)
        //    {
        //        var dbQuestion = await _context.Questions.FindAsync(question.Id);
        //        if (dbQuestion != null && question.UserAnswer != null && dbQuestion.CorrectAnswer == question.UserAnswer)
        //        {
        //            correctAnswers++;
        //        }
        //    }
        //    ViewBag.CorrectAnswers = correctAnswers;
        //    return View("QuizResult");
        //}
        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(List<AnswerVM> questions)
        {
            // 1. Pobierz aktualnie zalogowanego użytkownika
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            int correctAnswers = 0;

            foreach (var question in questions)
            {
                var dbQuestion = await _context.Questions.FindAsync(question.Id);
                if (dbQuestion == null) continue;

                // 2. Zlicz poprawne odpowiedzi
                if (!string.IsNullOrWhiteSpace(question.UserAnswer) &&
                    dbQuestion.CorrectAnswer == question.UserAnswer)
                {
                    correctAnswers++;
                }

                // 3. Zapisz odpowiedź użytkownika
                var userAnswer = new UserAnswerOnQuestion
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Question = dbQuestion,
                    userAnswer = question.UserAnswer
                };

                _context.Answers.Add(userAnswer); // Zakładam, że DbSet nazywa się "Answers"
            }

            await _context.SaveChangesAsync(); // 4. Zapisz wszystkie odpowiedzi w bazie

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

        [HttpGet]
        public async Task<IActionResult> GetUsersAndAnswers(Guid filmId)
        {
            var answers = await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .Where(a => a.Question.FilmId == filmId)
                .ToListAsync();

            var answersGroupedByUser = answers
                .GroupBy(a => a.UserId)
                .Select(group => new UserAnswersVM
                {
                    UserName = group.First().User.FirstName + " " + group.First().User.LastName,
                    Answers = group.Select(a => new AnswerVM
                    {
                        Id = a.Question.Id,
                        Content = a.Question.Content,
                        A = a.Question.A,
                        B = a.Question.B,
                        C = a.Question.C,
                        D = a.Question.D,
                        UserAnswer = a.userAnswer,
                        CorrectAnswer = a.Question.CorrectAnswer
                    }).ToList()
                })
                .ToList();

            return View(answersGroupedByUser);
        }

    }
}
