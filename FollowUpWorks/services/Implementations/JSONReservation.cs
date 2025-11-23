using FollowUpWorks.Models;
using FollowUpWorks.services.Abstractions;
using System.Text.Json;

namespace FollowUpWorks.services.Implementations
{
    public class JSONReservation : IJSONReservation
    {
        private const string DataFilePath = "C:\\Users\\quint\\OneDrive\\Documentos\\programacion\\program_software2025\\followUpProjectsSoftDevp\\FollowUpWorks\\Models\\ReservationClassSection\\Reservation.json";
       

        public List<ReservationClass> GetAll()
        {
            if (File.Exists(DataFilePath))
            {
                string jsonString = File.ReadAllText(DataFilePath);

                // --- Paso de Validación Clave ---
                // 1. Asegúrate de que no esté vacío.
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    // Si está vacío o solo tiene espacios, retorna una lista nueva
                    return new List<ReservationClass>();
                }

                // 2. Opcional, pero bueno: verifica si el primer carácter es el de una lista
                if (jsonString.TrimStart().StartsWith('['))
                {
                    try
                    {
                        // Solo si parece una lista, intenta deserializarla como lista
                        return JsonSerializer.Deserialize<List<ReservationClass>>(jsonString)
                               ?? new List<ReservationClass>();
                    }
                    catch (JsonException ex)
                    {
                        // Si falla la deserialización, loguea o maneja el error
                        // y retorna una lista vacía para evitar que la aplicación se caiga.
                        // Podrías retornar el error para depurar si es necesario.
                        Console.WriteLine($"Error de JSON al deserializar: {ex.Message}");
                        return new List<ReservationClass>();
                    }
                }
                // Si el archivo tiene contenido, pero no empieza con '[', es un formato incorrecto.
                // Retorna vacío para evitar el error de crash.
                return new List<ReservationClass>();
            }

            // Si el archivo no existe, retorna una lista nueva
            return new List<ReservationClass>();
        }

        public void SaveAll(List<ReservationClass> reservation)
        {
            string jsonString = JsonSerializer.Serialize(reservation, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DataFilePath, jsonString);
        }

        public void UpdateReservation(ReservationClass updatedReservation)
        {
            List<ReservationClass> reservation = GetAll();

            // 2. Buscar el índice (posición) de la receta a actualizar

            int indexToUpdate = reservation.FindIndex(r => r.idReservation == updatedReservation.idReservation);

            if (indexToUpdate != -1)
            {
                // 3. Reemplazar la receta antigua con la receta actualizada
                reservation[indexToUpdate] = updatedReservation;

                // 4. Guardar la lista completa (con la modificación) de vuelta al archivo JSON
                SaveAll(reservation);
            }
            else
            {

                throw new KeyNotFoundException($"Reservacion con ID {updatedReservation.idReservation} no encontrada.");
            }
        }
        public void DeleteReservation(Guid id)
        {
            List<ReservationClass> reservation = GetAll();

            // 2. Usar RemoveAll() para eliminar el elemento que coincida con el ID.
            //    RemoveAll devuelve el número de elementos eliminados.
            int itemsRemoved = reservation.RemoveAll(r => r.idReservation == id);

            if (itemsRemoved > 0)
            {
                // 3. Si se eliminó al menos un elemento, guardar la lista modificada
                //    de vuelta al archivo JSON.
                SaveAll(reservation);
            }
            else
            {

                Console.WriteLine($"Advertencia: Reservacion con ID {id} no encontrada para eliminar.");
            }
        }
    }
}
