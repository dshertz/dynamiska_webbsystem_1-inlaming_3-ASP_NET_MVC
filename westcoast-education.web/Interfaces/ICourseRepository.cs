using westcoast_education.web.Models;

namespace westcoast_education.web.Interfaces;

public interface ICourseRepository
    {
        Task<IList<Course>> ListAllAsync();
        Task<Course?> FindByIdAsync(int id);
        Task<Course?> FindByCourseNumberAsync(string courseNumber);
        Task<bool> AddAsync(Course course);        
        Task<bool> UpdateAsync(Course course);        
        Task<bool> DeleteAsync(Course course);
    }