using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre de requerido")]
        [StringLength(maximumLength:50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [Display (Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Tipo de transacción")]
        public TransactionType TransactionTypeId { get; set; }
        public  int UserId { get; set; }
    }
}
