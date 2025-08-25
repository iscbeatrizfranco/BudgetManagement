using BudgetManagement.Validations;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class AccountType//:IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        [FirstCapitalLetter]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Order { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(!string.IsNullOrEmpty(Name))
        //    {
        //        var firstLetter = Name[0].ToString();
        //        if (firstLetter != firstLetter.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayúscula", 
        //                new[] { nameof(Name) });
        //        }
        //    }
        //}
    }
}
