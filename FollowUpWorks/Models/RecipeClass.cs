using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class RecipeClass : iID
    {
        public Guid idRecipe { get; set; }
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public List<string>? Ingredients { get; set; } = new List<string>();
        public string? Instructions { get; set; } = string.Empty;
        public int? PreparationTimeMinutes { get; set; }
        public int? CookingTimeMinutes { get; set; }
        public int? Servings { get; set; }
        public string? DishType { get; set; } = string.Empty; // "Dulce", "Salado", "Entrada"
        public string? MainIngredientCategory { get; set; } = string.Empty; // "Carne", "Granos", "Vegetales", "Frutas"
    }
}
