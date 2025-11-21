using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }

        public string Bio { get; set; } = string.Empty;

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
