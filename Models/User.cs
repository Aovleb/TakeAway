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
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
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
    }
}
