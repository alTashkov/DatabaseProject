using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string? Content { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
