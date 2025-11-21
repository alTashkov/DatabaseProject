using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users = new List<User>();
    }
}
