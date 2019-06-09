using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supinfy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreationDate { get; set; }
        public Role Role { get; set; }

        public static List<User> LoadAll()
        {
            List<User> users;

            using (var context = new SupinfyContext())
            {
                users = context.Users.ToList();
            }

            return users;
        }

        public static User LoadById(int id)
        {
            User user;

            using (var context = new SupinfyContext())
            {
                user = context.Users.FirstOrDefault(u => u.Id == id);
            }

            return user;
        }

        public static User Authenticate(string email, string password)
        {
            User user;

            using (var context = new SupinfyContext())
            {
                user = context.Users.FirstOrDefault(u => u.Email == email.ToLower()
                                                    && BCrypt.Net.BCrypt.Verify(password, u.Password, false, BCrypt.Net.HashType.SHA384));
            }

            return user;
        }

        public bool Save()
        {
            try
            {
                using (var context = new SupinfyContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == Id);

                    if (user == null)
                    {
                        context.Users.Add(this);
                    }
                    else
                    {
                        user.Email = Email;
                        user.Password = Password;
                        user.FirstName = FirstName;
                        user.LastName = LastName;
                        user.CreationDate = CreationDate;
                        user.Role = Role;
                    }

                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public enum Role
    {
        Standard = 0,
        Admin = 1
    }
}
