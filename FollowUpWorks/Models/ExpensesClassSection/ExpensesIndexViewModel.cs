using FollowUpWorks.DTOs;
using FollowUpWorks.DTOs.ExpensesClassDTOSection;

namespace FollowUpWorks.Models.ExpensesClassSection
{
    public class ExpensesIndexViewModel
    {
        public List<ExpensesClassDTO> Expenses { get; set; } = new List<ExpensesClassDTO>();

        // Datos de Resumen
        public decimal TotalExpenses { get; set; }
        public List<ExpenseCategorySummaryDTO> ExpensesByCategory { get; set; } = new List<ExpenseCategorySummaryDTO>();

        // Datos de Filtro (para que la vista recuerde lo seleccionado)
        public List<string> Categories { get; set; } = new List<string>();
        public string SelectedCategory { get; set; } = "Todas";
        public int SelectedMonth { get; set; } = DateTime.Now.Month;
        public int SelectedYear { get; set; } = DateTime.Now.Year;
    }
}
