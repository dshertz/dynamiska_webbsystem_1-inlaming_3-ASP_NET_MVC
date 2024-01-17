using System.ComponentModel.DataAnnotations;

namespace westcoast_education.web.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; } = "";
        public string CourseTitle { get; set; } = "";
        public string CourseNumber { get; set; } = "";
        public DateTime CourseStartDate { get; set; }
        public int CourseLengthInWeeks { get; set; }
    }
}