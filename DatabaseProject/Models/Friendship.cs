using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class Friendship
    {
        public int FriendshipId { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("User1")]
        public int UserID1 { get; set; }
        public User User1 { get; set; } = null!;

        [ForeignKey("User2")]
        public int UserID2 { get; set; }
        public User User2 { get; set; } = null!;
    }
}
