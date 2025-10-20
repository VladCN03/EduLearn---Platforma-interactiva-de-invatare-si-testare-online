using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationalPlatform_Backend.Models
{
    public class Test
    {
        public int TestID { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!; // "activ" sau "inactiv"

        public string? Attachment { get; set; }

        public string? Questions { get; set; }  // JSON string

        public string? Comments { get; set; }

        public int Duration { get; set; }

        public int Rating { get; set; }

        public int maxGrade { get; set; }

        public ICollection<TestResult>? TestResults { get; set; }
    }
}
