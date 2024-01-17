using Microsoft.AspNetCore.Mvc;
using westcoast_education.web.Interfaces;
using westcoast_education.web.Models;
using westcoast_education.web.ViewModels;

namespace westcoast_education.web.Controllers;

[Route("persons/admin")]
public class PersonsAdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public PersonsAdminController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var persons = await _unitOfWork.PersonRepository.ListAllAsync();

            var model = persons.Select(v => new PersonListViewModel
            {
                PersonId = v.PersonId,
                PersonCategory = v.PersonCategory,
                SocialSecurityNumber = v.SocialSecurityNumber,
                Name = v.Name,
                Email = v.Email,
                Address = v.Address,
                PhoneNumber = v.PhoneNumber
            }).ToList();

            return View("Index", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när vi skulle hämta alla personer",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        };
    }

    [HttpGet("create")]
    public IActionResult Edit()
    {
        var person = new PersonPostViewModel();
        return View("Create", person);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(PersonPostViewModel person)
    {
        try
        {
            if (!ModelState.IsValid) return View("Create", person);

            var exists = await _unitOfWork.PersonRepository.FindBySocialSecurityNumberAsync((long)person.SocialSecurityNumber!);

            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när personen skulle sparas!",
                    ErrorMessage = "Det gick inte så bra!"
                };

                return View("_Error", error);
            }

            var personToAdd = new Person
            {
                PersonCategory = person.PersonCategory,
                SocialSecurityNumber = (long)person.SocialSecurityNumber!,
                Name = person.Name,
                Email = person.Email,
                Address = person.Address,
                PhoneNumber = person.PhoneNumber
            };

            if (await _unitOfWork.PersonRepository.AddAsync(personToAdd))
            {
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var saveError = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när personen skulle sparas!",
                ErrorMessage = $"Det inträffade ett fel när personen med personnummer {person.SocialSecurityNumber} skulle sparas"
            };

            return View("_Error", saveError);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när personen skulle sparas!",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpGet("edit/{personId}")]
    public async Task<IActionResult> Edit(int personId)
    {
        try
        {
            var result = await _unitOfWork.PersonRepository.FindByIdAsync(personId);

            if (result is null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när vi skulle hämta en person för editering",
                    ErrorMessage = $"Vi hittar ingen person med id {personId}"
                };

                return View("_Error", error);
            }

            var model = new PersonUpdateViewModel
            {
                PersonId = result.PersonId,
                PersonCategory = result.PersonCategory,
                SocialSecurityNumber = result.SocialSecurityNumber,
                Name = result.Name,
                Email = result.Email,
                Address = result.Address,
                PhoneNumber = result.PhoneNumber
            };

            return View("Edit", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när vi hämtar person för redigering",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpPost("edit/{personId}")]
    public async Task<IActionResult> Edit(int personId, PersonUpdateViewModel person)
    {
        try
        {
            if (!ModelState.IsValid) return View("Edit", person);

            var personToUpdate = await _unitOfWork.PersonRepository.FindByIdAsync(personId);

            if (personToUpdate is null) return RedirectToAction(nameof(Index));

            personToUpdate.PersonId = person.PersonId;
            personToUpdate.PersonCategory = person.PersonCategory;
            personToUpdate.SocialSecurityNumber = (long)person.SocialSecurityNumber!;
            personToUpdate.Name = person.Name;
            personToUpdate.Email = person.Email;
            personToUpdate.Address = person.Address;
            personToUpdate.PhoneNumber = person.PhoneNumber;

            if (await _unitOfWork.PersonRepository.UpdateAsync(personToUpdate))
            {
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när personen skulle sparas!",
                ErrorMessage = $"Ett fel inträffade när vi skulle uppdatera personen med Id {personToUpdate.PersonId}"
            };

            return View("_Error", error);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när vi skulle spara personen",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }

    }

    [Route("delete/{personId}")]
    public async Task<IActionResult> Delete(int personId)
    {
        try
        {
            var personToDelete = await _unitOfWork.PersonRepository.FindByIdAsync(personId);

            if (personToDelete is null) return RedirectToAction(nameof(Index));

            if (await _unitOfWork.PersonRepository.DeleteAsync(personToDelete))
            {
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när personen skulle raderas!",
                ErrorMessage = $"Ett fel inträffade när personen med personnummer {personToDelete.SocialSecurityNumber} skulle tas bort"
            };

            return View("_Error", error);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när personen skulle raderas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }
}