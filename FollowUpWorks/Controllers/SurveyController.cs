using AutoMapper;
using FollowUpWorks.DTOs;
using FollowUpWorks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace FollowUpWorks.Controllers
{
    public class SurveyController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private const string SurveysKey = "Surveys";
        private const string ResponsesKey = "SurveyResponses";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public SurveyController(IMemoryCache cache, IMapper mapper)
        {
            _cache = cache;
            _mapper = mapper;
        }

        private List<SurveyClass> GetSurveys()
        {
            return _cache.GetOrCreate(SurveysKey, e =>
            {
                e.SlidingExpiration = TimeSpan.FromHours(24);
                return new List<SurveyClass>();
            }) ?? new List<SurveyClass>();
        }

        private List<SurveyResponse> GetResponses()
        {
            return _cache.GetOrCreate(ResponsesKey, e =>
            {
                e.SlidingExpiration = TimeSpan.FromHours(24);
                return new List<SurveyResponse>();
            }) ?? new List<SurveyResponse>();
        }

        private void SaveSurveys(List<SurveyClass> surveys)
        {
            _cache.Set(SurveysKey, surveys, TimeSpan.FromHours(24));
        }

        private void SaveResponses(List<SurveyResponse> responses)
        {
            _cache.Set(ResponsesKey, responses, TimeSpan.FromHours(24));
        }

        // GET: Survey/Index
        public IActionResult Index()
        {
            var surveys = GetSurveys();
            var responses = GetResponses();

            var dtos = _mapper.Map<List<SurveyClassDTO>>(surveys);

            ViewBag.ResponseCounts = surveys.ToDictionary(
                s => s.Id,
                s => responses.Count(r => r.SurveyId == s.Id)
            );

            return View(dtos);
        }

        // GET: Survey/Create
        public IActionResult Create()
        {
            return View(new SurveyClassDTO());
        }

        // POST: Survey/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SurveyClassDTO dto, string questionsJson)
        {
            if (string.IsNullOrEmpty(dto.Title))
            {
                ModelState.AddModelError("Title", "El título es requerido");
                return View(dto);
            }

            // Parsear preguntas del JSON
            var questionDtos = new List<SurveyQuestionDTO>();
            if (!string.IsNullOrEmpty(questionsJson))
            {
                try
                {
                    questionDtos = JsonSerializer.Deserialize<List<SurveyQuestionDTO>>(questionsJson, JsonOptions)
                        ?? new List<SurveyQuestionDTO>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing questions: {ex.Message}");
                }
            }

            // Mapear a modelo
            var survey = new SurveyClass
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description ?? "",
                CreatedAt = DateTime.UtcNow,
                IsActive = dto.IsActive,
                Questions = _mapper.Map<List<SurveyQuestion>>(questionDtos)
            };

            var surveys = GetSurveys();
            surveys.Add(survey);
            SaveSurveys(surveys);

            TempData["SuccessMessage"] = "Encuesta creada exitosamente";
            return RedirectToAction(nameof(Index));
        }

        // GET: Survey/Edit/5
        public IActionResult Edit(Guid id)
        {
            var surveys = GetSurveys();
            var survey = surveys.FirstOrDefault(s => s.Id == id);

            if (survey == null)
            {
                TempData["ErrorMessage"] = "Encuesta no encontrada";
                return RedirectToAction(nameof(Index));
            }

            var dto = _mapper.Map<SurveyClassDTO>(survey);
            return View(dto);
        }

        // POST: Survey/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, SurveyClassDTO dto, string questionsJson)
        {
            var surveys = GetSurveys();
            var survey = surveys.FirstOrDefault(s => s.Id == id);

            if (survey == null)
            {
                TempData["ErrorMessage"] = "Encuesta no encontrada";
                return RedirectToAction(nameof(Index));
            }

            // Actualizar propiedades básicas
            survey.Title = dto.Title;
            survey.Description = dto.Description ?? "";
            survey.IsActive = dto.IsActive;

            // Parsear y mapear preguntas
            if (!string.IsNullOrEmpty(questionsJson))
            {
                try
                {
                    var questionDtos = JsonSerializer.Deserialize<List<SurveyQuestionDTO>>(questionsJson, JsonOptions)
                        ?? new List<SurveyQuestionDTO>();
                    survey.Questions = _mapper.Map<List<SurveyQuestion>>(questionDtos);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing questions: {ex.Message}");
                }
            }

            SaveSurveys(surveys);
            TempData["SuccessMessage"] = "Encuesta actualizada";
            return RedirectToAction(nameof(Index));
        }

        // POST: Survey/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            var surveys = GetSurveys();
            var survey = surveys.FirstOrDefault(s => s.Id == id);

            if (survey != null)
            {
                surveys.Remove(survey);
                SaveSurveys(surveys);

                var responses = GetResponses();
                responses.RemoveAll(r => r.SurveyId == id);
                SaveResponses(responses);

                TempData["SuccessMessage"] = "Encuesta eliminada";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Survey/Respond/5
        public IActionResult Respond(Guid id)
        {
            var surveys = GetSurveys();
            var survey = surveys.FirstOrDefault(s => s.Id == id && s.IsActive);

            if (survey == null)
            {
                TempData["ErrorMessage"] = "Encuesta no disponible";
                return RedirectToAction(nameof(Index));
            }

            var dto = _mapper.Map<SurveyClassDTO>(survey);
            return View(dto);
        }

        // POST: Survey/SubmitResponse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitResponse(Guid surveyId, string answersJson)
        {
            var surveys = GetSurveys();
            var survey = surveys.FirstOrDefault(s => s.Id == surveyId);

            if (survey == null)
            {
                return Json(new { success = false, message = "Encuesta no encontrada" });
            }

            try
            {
                var answerDtos = JsonSerializer.Deserialize<List<QuestionResponseDTO>>(answersJson, JsonOptions)
                    ?? new List<QuestionResponseDTO>();

                var response = new SurveyResponse
                {
                    Id = Guid.NewGuid(),
                    SurveyId = surveyId,
                    SubmittedAt = DateTime.UtcNow,
                    Answers = _mapper.Map<List<QuestionResponse>>(answerDtos)
                };

                var responses = GetResponses();
                responses.Add(response);
                SaveResponses(responses);

                TempData["SuccessMessage"] = "¡Gracias por tu respuesta!";
                return Json(new { success = true, redirectUrl = Url.Action("ThankYou") });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving response: {ex.Message}");
                return Json(new { success = false, message = "Error al procesar la respuesta" });
            }
        }

        // GET: Survey/ThankYou
        public IActionResult ThankYou()
        {
            return View();
        }

        // GET: Survey/Dashboard/5
        public IActionResult Dashboard(Guid id)
        {
            var surveys = GetSurveys();
            var survey = surveys.FirstOrDefault(s => s.Id == id);

            if (survey == null)
            {
                TempData["ErrorMessage"] = "Encuesta no encontrada";
                return RedirectToAction(nameof(Index));
            }

            var allResponses = GetResponses();
            var surveyResponses = allResponses.Where(r => r.SurveyId == id).ToList();

            var analytics = new SurveyAnalyticsDTO
            {
                SurveyId = survey.Id,
                SurveyTitle = survey.Title,
                TotalResponses = surveyResponses.Count,
                QuestionStats = survey.Questions.Select(q =>
                {
                    var qResponses = surveyResponses
                        .SelectMany(r => r.Answers)
                        .Where(a => a.QuestionId == q.Id)
                        .ToList();

                    var stats = new QuestionAnalyticsDTO
                    {
                        QuestionId = q.Id,
                        QuestionText = q.QuestionText,
                        Type = q.Type.ToString()
                    };

                    switch (q.Type)
                    {
                        case QuestionType.SingleChoice:
                        case QuestionType.MultipleChoice:
                            stats.OptionCounts = q.Options
                                .Select((opt, idx) => new { opt, idx })
                                .ToDictionary(
                                    x => x.opt,
                                    x => qResponses.Count(r => r.SelectedOptions.Contains(x.idx))
                                );
                            break;
                        case QuestionType.Text:
                            stats.TextResponses = qResponses
                                .Select(r => r.TextAnswer)
                                .Where(t => !string.IsNullOrEmpty(t))
                                .ToList();
                            break;
                        case QuestionType.Rating:
                            var ratings = qResponses
                                .Where(r => r.RatingValue.HasValue)
                                .Select(r => r.RatingValue!.Value)
                                .ToList();
                            stats.AverageRating = ratings.Any() ? ratings.Average() : null;
                            stats.OptionCounts = Enumerable.Range(1, 5)
                                .ToDictionary(
                                    i => i.ToString(),
                                    i => ratings.Count(r => r == i)
                                );
                            break;
                    }

                    return stats;
                }).ToList()
            };

            return View(analytics);
        }
    }
}