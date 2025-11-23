using System.Text.Json.Serialization;

namespace FollowUpWorks.DTOs
{
    public class SurveyClassDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("questions")]
        public List<SurveyQuestionDTO> Questions { get; set; } = new();

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class SurveyQuestionDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = "SingleChoice";

        [JsonPropertyName("options")]
        public List<string> Options { get; set; } = new();

        [JsonPropertyName("isRequired")]
        public bool IsRequired { get; set; } = true;
    }

    public class SurveyResponseDTO
    {
        [JsonPropertyName("surveyId")]
        public Guid SurveyId { get; set; }

        [JsonPropertyName("answers")]
        public List<QuestionResponseDTO> Answers { get; set; } = new();
    }

    public class QuestionResponseDTO
    {
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("selectedOptions")]
        public List<int> SelectedOptions { get; set; } = new();

        [JsonPropertyName("textAnswer")]
        public string TextAnswer { get; set; } = string.Empty;

        [JsonPropertyName("ratingValue")]
        public int? RatingValue { get; set; }
    }

    // Para el Dashboard
    public class SurveyAnalyticsDTO
    {
        [JsonPropertyName("surveyId")]
        public Guid SurveyId { get; set; }

        [JsonPropertyName("surveyTitle")]
        public string SurveyTitle { get; set; } = string.Empty;

        [JsonPropertyName("totalResponses")]
        public int TotalResponses { get; set; }

        [JsonPropertyName("questionStats")]
        public List<QuestionAnalyticsDTO> QuestionStats { get; set; } = new();
    }

    public class QuestionAnalyticsDTO
    {
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("optionCounts")]
        public Dictionary<string, int> OptionCounts { get; set; } = new();

        [JsonPropertyName("textResponses")]
        public List<string> TextResponses { get; set; } = new();

        [JsonPropertyName("averageRating")]
        public double? AverageRating { get; set; }
    }
}
