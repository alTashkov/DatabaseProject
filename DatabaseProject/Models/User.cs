namespace DatabaseProject.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string? Username { get; set; } = null;

        public string? Email { get; set; } = null;

        public string? Password { get; set; } = string.Empty;

        public Profile Profile { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<Friendship> FriendshipsInitiated { get; set; } = new List<Friendship>();

        public ICollection<Friendship> FriendshipsReceived { get; set; } = new List<Friendship>();

        public ICollection<Group> Groups = new List<Group>();
    }
}
