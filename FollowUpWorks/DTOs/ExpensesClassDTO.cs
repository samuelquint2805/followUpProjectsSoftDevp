using System.ComponentModel.DataAnnotations;

namespace FollowUpWorks.DTOs
{
    public class ExpensesClassDTO
    {
        public Guid IdExpense { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name = "Categoría")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es requerido")]
        
        [Display(Name = "Monto")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; } = string.Empty;

        
    }
}
