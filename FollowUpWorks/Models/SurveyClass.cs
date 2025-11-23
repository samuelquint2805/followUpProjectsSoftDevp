using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class SurveyClass : iID
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<SurveyQuestion> Questions { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }

    public class SurveyQuestion
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.SingleChoice;
        public List<string> Options { get; set; } = new();
        public bool IsRequired { get; set; } = true;
    }

    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        Text,
        Rating
    }

    public class SurveyResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SurveyId { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public List<QuestionResponse> Answers { get; set; } = new();
        public string RespondentId { get; set; } = string.Empty; // Opcional
    }

    public class QuestionResponse
    {
        public Guid QuestionId { get; set; }
        public List<int> SelectedOptions { get; set; } = new(); // Índices
        public string TextAnswer { get; set; } = string.Empty;
        public int? RatingValue { get; set; }
    }
}
