using Microsoft.EntityFrameworkCore;
using westcoast_education.web.Data;
using westcoast_education.web.Interfaces;
using westcoast_education.web.Models;

namespace westcoast_education.web.Repository;

public class PersonRepository : IPersonRepository
{
    private readonly WestcoastEducationContext _context;
    public PersonRepository(WestcoastEducationContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(Person person)
    {
        try
        {
            await _context.Persons.AddAsync(person);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> DeleteAsync(Person person)
    {
        try
        {
            _context.Persons.Remove(person);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<Person?> FindByIdAsync(int id)
    {
        return await _context.Persons.FindAsync(id);
    }

    public async Task<Person?> FindBySocialSecurityNumberAsync(long socialSecurityNumber)
    {
        return await _context.Persons.SingleOrDefaultAsync(c => c.SocialSecurityNumber.ToString().Trim() == socialSecurityNumber.ToString().Trim());
    }

    public async Task<IList<Person>> ListAllAsync()
    {
        return await _context.Persons.ToListAsync();
    }

    public Task<bool> UpdateAsync(Person person)
    {
        try
        {
            _context.Persons.Update(person);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}