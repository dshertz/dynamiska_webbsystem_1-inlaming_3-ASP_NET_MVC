using Microsoft.AspNetCore.Mvc;
using westcoast_education.web.Interfaces;
using westcoast_education.web.Models;
using westcoast_education.web.ViewModels;

namespace westcoast_education.web.Controllers;

[Route("courses/admin")]
public class CoursesAdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CoursesAdminController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var courses = await _unitOfWork.CourseRepository.ListAllAsync();

            var model = courses.Select(v => new CourseListViewModel
            {
                CourseId = v.CourseId,
                CourseName = v.CourseName,
                CourseTitle = v.CourseTitle,
                CourseNumber = v.CourseNumber,
                CourseStartDate = v.CourseStartDate,
                CourseLengthInWeeks = v.CourseLengthInWeeks
            }).ToList();

            return View("Index", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när vi skulle hämta alla kurser",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }


    [HttpGet("create")]
    public IActionResult Edit()
    {
        var course = new CoursePostViewModel();
        return View("Create", course);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CoursePostViewModel course)
    {
        try
        {
            if (!ModelState.IsValid) return View("Create", course);

            var exists = await _unitOfWork.CourseRepository.FindByCourseNumberAsync(course.CourseNumber);

            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när kursen skulle sparas!",
                    ErrorMessage = "Det gick inte så bra!"
                };

                return View("_Error", error);
            }

            var courseToAdd = new Course
            {
                CourseName = course.CourseName,
                CourseTitle = course.CourseTitle,
                CourseNumber = course.CourseNumber,
                CourseStartDate = course.CourseStartDate,
                CourseLengthInWeeks = (int)course.CourseLengthInWeeks!
            };

            if (await _unitOfWork.CourseRepository.AddAsync(courseToAdd))
            {
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var saveError = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle sparas!",
                ErrorMessage = $"Det inträffade ett fel när kursen med kursnummer {course.CourseNumber} skulle sparas"
            };

            return View("_Error", saveError);

        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle sparas!",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }


    [HttpGet("edit/{courseId}")]
    public async Task<IActionResult> Edit(int courseId)
    {
        try
        {
            var result = await _unitOfWork.CourseRepository.FindByIdAsync(courseId);

            if (result is null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när vi skulle hämta en kurs för editering",
                    ErrorMessage = $"Vi hittar ingen person med id {courseId}"
                };

                return View("_Error", error);
            }

            var model = new CourseUpdateViewModel
            {
                CourseId = result.CourseId,
                CourseName = result.CourseName,
                CourseTitle = result.CourseTitle,
                CourseNumber = result.CourseNumber,
                CourseStartDate = result.CourseStartDate,
                CourseLengthInWeeks = result.CourseLengthInWeeks
            };

            return View("Edit", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när vi hämtar kurs för redigering",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }

    }

    [HttpPost("edit/{courseId}")]
    public async Task<IActionResult> Edit(int courseId, CourseUpdateViewModel course)
    {
        try
        {
            if (!ModelState.IsValid) return View("Edit", course);

            var courseToUpdate = await _unitOfWork.CourseRepository.FindByIdAsync(courseId);

            if (courseToUpdate is null) return RedirectToAction(nameof(Index));

            courseToUpdate.CourseId = course.CourseId;
            courseToUpdate.CourseName = course.CourseName;
            courseToUpdate.CourseTitle = course.CourseTitle;
            courseToUpdate.CourseNumber = course.CourseNumber;
            courseToUpdate.CourseStartDate = course.CourseStartDate;
            courseToUpdate.CourseLengthInWeeks = (int)course.CourseLengthInWeeks!;

            if (await _unitOfWork.CourseRepository.UpdateAsync(courseToUpdate))
            {
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle sparas!",
                ErrorMessage = $"Ett fel inträffade när vi skulle uppdatera kursen med Id {courseToUpdate.CourseId}"
            };

            return View("_Error", error);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när vi skulle spara kursen",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [Route("delete/{courseId}")]
    public async Task<IActionResult> Delete(int courseId)
    {
        try
        {
            var courseToDelete = await _unitOfWork.CourseRepository.FindByIdAsync(courseId);

            if (courseToDelete is null) return RedirectToAction(nameof(Index));

            if (await _unitOfWork.CourseRepository.DeleteAsync(courseToDelete))
            {
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle raderas!",
                ErrorMessage = $"Ett fel inträffade när kusrsen med kursnummer {courseToDelete.CourseNumber} skulle tas bort"
            };

            return View("_Error", error);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle raderas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }
}