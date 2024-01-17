using Microsoft.EntityFrameworkCore;
using westcoast_education.web.Data;
using westcoast_education.web.Interfaces;
using westcoast_education.web.Models;

namespace westcoast_education.web.Repository;

public class CourseRepository : ICourseRepository
{
    private readonly WestcoastEducationContext _context;
    public CourseRepository(WestcoastEducationContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(Course course)
    {
        try
        {
            await _context.Courses.AddAsync(course);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> DeleteAsync(Course course)
    {
        try
        {
            _context.Courses.Remove(course);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<Course?> FindByCourseNumberAsync(string courseNumber)
    {
        return await _context.Courses.SingleOrDefaultAsync(c => c.CourseNumber.Trim().ToLower() == courseNumber.Trim().ToLower());
    }

    public async Task<Course?> FindByIdAsync(int id)
    {
        return await _context.Courses.FindAsync(id);
    }

    public async Task<IList<Course>> ListAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public Task<bool> UpdateAsync(Course course)
    {
        try
        {
            _context.Courses.Update(course);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}