using System.ComponentModel.DataAnnotations;
using TakeAway.DAL.Interfaces;
using TakeAway.Validations;

namespace TakeAway.Models
{
    public abstract class User
    {
        private int id;
        private string email;
        private string password;
        private string confirmPassword;
        private bool conditions;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [Required(ErrorMessage ="An email address is required.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage="Address email invalide.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage= "Address email invalide !")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        [Required(ErrorMessage="A password is required.")]
        [Display(Name="Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; }
        }
        public bool Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }

        public User(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
        public User() { }
        public static async Task<User> GetUserAsync(IUserDAL userDAL, string email, string password)
        {
            return await userDAL.GetUserAsync(email, password);
        }
    }
}
