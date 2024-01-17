using westcoast_education.web.Interfaces;
using westcoast_education.web.Repository;

namespace westcoast_education.web.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly WestcoastEducationContext _context;
    public UnitOfWork(WestcoastEducationContext context)
    {
        _context = context;
    }

    public ICourseRepository CourseRepository => new CourseRepository(_context);

    public IPersonRepository PersonRepository => new PersonRepository(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}