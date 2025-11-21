using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class Post
    {
        public int PostId { get; set; }

        public string? Content { get; set; }

        public string? MediaURL { get; set; }

        public DateTime DateCreated { get; set; }

        public ICollection<Comment> Comments = new List<Comment>();

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
