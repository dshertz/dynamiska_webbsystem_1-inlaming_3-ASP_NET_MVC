namespace westcoast_education.web.Interfaces;

public interface IUnitOfWork
{
    ICourseRepository CourseRepository { get; }
    IPersonRepository PersonRepository { get; }
    Task<bool> Complete();
}