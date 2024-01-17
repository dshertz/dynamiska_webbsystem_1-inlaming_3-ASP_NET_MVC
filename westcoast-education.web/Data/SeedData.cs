using System.Text.Json;
using westcoast_education.web.Models;

namespace westcoast_education.web.Data;

public static class SeedData
{
    public static async Task LoadCourseData(WestcoastEducationContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.Courses.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/courses.json");
        
        var courses = JsonSerializer.Deserialize<List<Course>>(json, options);

        if (courses is not null && courses.Count > 0)
        {
            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadPersonData(WestcoastEducationContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.Persons.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/persons.json");
        
        var persons = JsonSerializer.Deserialize<List<Person>>(json, options);

        if (persons is not null && persons.Count > 0)
        {
            await context.Persons.AddRangeAsync(persons);
            await context.SaveChangesAsync();
        }
    }
}