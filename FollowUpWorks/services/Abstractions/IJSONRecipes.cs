using FollowUpWorks.Models;

namespace FollowUpWorks.services.Abstractions
{
    public interface IJSONRecipes
    {
        List<RecipeClass> GetAll();
        void SaveAll(List<RecipeClass> recipe);
        void UpdateRecipe(RecipeClass updatedRecipe);
        void DeleteRecipe(Guid id);
    }
}
