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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Required(ErrorMessage ="Email address is required.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage="Address email invalide.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage= "Address email invalide !")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [Required(ErrorMessage="Password is required.")]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return password; }
            set { password = value; }
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
