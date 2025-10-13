using BudgetManagement.Validations;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        [StringLength(maximumLength:50)]
        [FirstCapitalLetter]
        public string Name { get; set; }

        [Display(Name = "Tipos de cuentas")]
        public int AccountTypeId { get; set; }

        public decimal Balance { get; set; }

        [StringLength(maximumLength:1000)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public string AccountType { get; set; }
    }
}
