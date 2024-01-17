namespace westcoast_education.web.ViewModels;

public class CourseListViewModel
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = "";
    public string CourseTitle { get; set; } = "";
    public string CourseNumber { get; set; } = "";
    public DateTime CourseStartDate { get; set; }
    public int CourseLengthInWeeks { get; set; }
}