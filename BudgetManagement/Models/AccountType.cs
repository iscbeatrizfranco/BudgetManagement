using BudgetManagement.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class AccountType//:IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        [FirstCapitalLetter]
        [Remote(action:"VerifyExistsAccountType", controller:"AccountsTypes")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int DisplayOrder { get; set; }

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
