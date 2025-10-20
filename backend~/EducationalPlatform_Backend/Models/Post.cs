using System;

namespace EducationalPlatform_Backend.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Author { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string? Attachment { get; set; }

        public int UserID { get; set; }        // NOU
        public User? User { get; set; }
    }
}
