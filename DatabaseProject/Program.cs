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
                context.Users.Add(user1);
                context.SaveChanges();

                var profile1 = new Profile() { UserId = user1.UserId, Bio = "Test bio for user1"};
                context.Profiles.Add(profile1);
                context.SaveChanges();

                var post1 = new Post() { Content = "Test poster", UserId = user1.UserId, User = user1 };
                context.Posts.Add(post1);
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

                Console.ReadLine();
            }
        }
    }
}
