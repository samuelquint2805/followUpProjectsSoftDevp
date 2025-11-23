using FollowUpWorks.Models;
using FollowUpWorks.services.Abstractions;
using System.Text.Json;

namespace FollowUpWorks.services.Implementations
{
    public class JSONRecipes : IJSONRecipes
    {
        private const string DataFilePath = "C:\\Users\\quint\\OneDrive\\Documentos\\programacion\\program_software2025\\followUpProjectsSoftDevp\\FollowUpWorks\\Models\\RecipeClassSection\\recipes.json";

        public List<RecipeClass> GetAll()
        {
            if (File.Exists(DataFilePath))
            {
                string jsonString = File.ReadAllText(DataFilePath);

                // --- Paso de Validación Clave ---
                // 1. Asegúrate de que no esté vacío.
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    // Si está vacío o solo tiene espacios, retorna una lista nueva
                    return new List<RecipeClass>();
                }

                // 2. Opcional, pero bueno: verifica si el primer carácter es el de una lista
                if (jsonString.TrimStart().StartsWith('['))
                {
                    try
                    {
                        // Solo si parece una lista, intenta deserializarla como lista
                        return JsonSerializer.Deserialize<List<RecipeClass>>(jsonString)
                               ?? new List<RecipeClass>();
                    }
                    catch (JsonException ex)
                    {
                        // Si falla la deserialización, loguea o maneja el error
                        // y retorna una lista vacía para evitar que la aplicación se caiga.
                        // Podrías retornar el error para depurar si es necesario.
                        Console.WriteLine($"Error de JSON al deserializar: {ex.Message}");
                        return new List<RecipeClass>();
                    }
                }
                // Si el archivo tiene contenido, pero no empieza con '[', es un formato incorrecto.
                // Retorna vacío para evitar el error de crash.
                return new List<RecipeClass>();
            }

            // Si el archivo no existe, retorna una lista nueva
            return new List<RecipeClass>();
        }

        public void SaveAll(List<RecipeClass> recipes)
        {
            string jsonString = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DataFilePath, jsonString);
        }

        public void UpdateRecipe(RecipeClass updatedRecipe)
        {
            // 1. Cargar toda la lista del archivo JSON
            List<RecipeClass> recipes = GetAll();

            // 2. Buscar el índice (posición) de la receta a actualizar
        
            int indexToUpdate = recipes.FindIndex(r => r.idRecipe == updatedRecipe.idRecipe);

            if (indexToUpdate != -1)
            {
                // 3. Reemplazar la receta antigua con la receta actualizada
                recipes[indexToUpdate] = updatedRecipe;

                // 4. Guardar la lista completa (con la modificación) de vuelta al archivo JSON
                SaveAll(recipes);
            }
            else
            {
                
                throw new KeyNotFoundException($"Receta con ID {updatedRecipe.idRecipe} no encontrada.");
            }

        }
        public void DeleteRecipe(Guid id)
        {
            // 1. Cargar toda la lista del archivo JSON
            List<RecipeClass> recipes = GetAll();

            // 2. Usar RemoveAll() para eliminar el elemento que coincida con el ID.
            //    RemoveAll devuelve el número de elementos eliminados.
            int itemsRemoved = recipes.RemoveAll(r => r.idRecipe == id);

            if (itemsRemoved > 0)
            {
                // 3. Si se eliminó al menos un elemento, guardar la lista modificada
                //    de vuelta al archivo JSON.
                SaveAll(recipes);
            }
            else
            {
               
                Console.WriteLine($"Advertencia: Receta con ID {id} no encontrada para eliminar.");
            }
        }
    }
}
