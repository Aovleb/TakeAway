using System.ComponentModel.DataAnnotations;
using TakeAway.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TakeAway.ViewModels
{
    public class SignUpViewModel
    {

        [Required(ErrorMessage = "You must accept the terms and conditions.")]
        [Display(Name = "Accept terms and conditions")]
        public bool Conditions { get; set; }


        [Required(ErrorMessage = "Password confirmation is required.")]
        [Display(Name = "Confirm password")]
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
                modelState.AddModelError("Conditions", "You must accept the terms and conditions.");
            }


            if (!string.IsNullOrEmpty(password) && ConfirmPassword != password)
            {
                isValid = false;

                modelState.AddModelError("ConfirmPassword", "The passwords do not match.");
            }
            return isValid;
        }
    }
}