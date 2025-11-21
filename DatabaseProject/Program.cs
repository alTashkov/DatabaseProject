using DatabaseProject.Data;
using DatabaseProject.Models;

namespace DatabaseProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SocialMediaContext())
            {
                context.Database.EnsureCreated();
             
                var user1 = new User() { Username = "alextashkov", Email = "alextashkov@gmail.com"};
                var profile1 = new Profile() { UserId = user1.UserId, Bio = "Test bio for user1" };
                
                var post1 = new Post() { Content = "Test post", UserId = user1.UserId, User = user1 };

                context.Add(user1);
                context.Add(post1);
                context.Add(profile1);

                context.SaveChanges();

                foreach (var u in context.Users)
                {
                    Console.WriteLine($"Username: {u.Username}, Email: {u.Email}");
                }

                foreach (var p in context.Posts)
                {
                    Console.WriteLine($"Content: {p.Content}");
                }

                foreach (var p in context.Profiles)
                {
                    Console.WriteLine($"Profile1: {p.Bio}");
                }
            }
        }
    }
}
