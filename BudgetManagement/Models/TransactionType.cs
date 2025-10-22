using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public enum TransactionType
    {
        [Display(Name = "Ingreso")]
        Income = 1,

        [Display(Name = "Gasto")]
        Expense = 2
    }
}
