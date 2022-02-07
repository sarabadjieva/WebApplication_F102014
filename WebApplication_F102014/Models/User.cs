using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication_F102014.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class EditableUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UsersContext : DbContext
    {
        public UsersContext() : base("WebsiteDB") { }

        public DbSet<User> Users { get; set; }
    }
}