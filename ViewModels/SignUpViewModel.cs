using System.ComponentModel.DataAnnotations;
using TakeAway.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TakeAway.ViewModels
{
    public class SignUpViewModel
    {

        [Required(ErrorMessage = "Vous devez accepter les conditions générales.")]
        [Display(Name = "Accepter les conditions générales")]
        public bool Conditions { get; set; }


        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [Display(Name = "Confirmer le mot de passe")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        public Client? Client { get; set; }
        public RestaurantOwner? RestaurantOwner { get; set; }


        public bool IsValid(ModelStateDictionary modelState, string password)
        {
            return GetValidationErrors(modelState, password);
        }


        public bool GetValidationErrors(ModelStateDictionary modelState, string password)
        {
            bool isValid = true;
            if (!Conditions)
            {
                isValid = false;
                modelState.AddModelError("Conditions", "Vous devez accepter les conditions générales.");
            }


            if (!string.IsNullOrEmpty(password) && ConfirmPassword != password)
            {
                isValid = false;

                modelState.AddModelError("ConfirmPassword", "Les mots de passe ne correspondent pas.");
            }
            return isValid;
        }
    }
}