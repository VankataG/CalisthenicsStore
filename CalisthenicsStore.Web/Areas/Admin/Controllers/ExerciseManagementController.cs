using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;
using static CalisthenicsStore.Common.Constants.Notifications;
using System;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class ExerciseManagementController : BaseAdminController
    {
        private readonly IExerciseManagementService exerciseService;

        private readonly ILogger<ExerciseManagementController> logger;

        public ExerciseManagementController(IExerciseManagementService exerciseService, ILogger<ExerciseManagementController> logger)
        {
            this.exerciseService = exerciseService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ExerciseManagementIndexViewModel> allExercises
                = await this.exerciseService.GetExerciseBoardDataAsync();

            return View(allExercises);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ExerciseCreateViewModel model = new ExerciseCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool isSuccess = await exerciseService.AddExerciseAsync(model);

                if (!isSuccess)
                {
                    TempData[ErrorMessageKey] = "Error occurred while adding the exercise!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Exercise added successfully!";
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] =
                    "Unexpected error occured while adding the exercise! Please contact the developer team.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                ExerciseCreateViewModel? model = await exerciseService.GetEditableExerciseAsync(id);

                if (model == null)
                { 
                    logger.LogWarning("Attempted to edit product with ID {ProductId}, but it was not found.", id);

                    TempData[ErrorMessageKey] = "Exercise does not exist!";

                    return RedirectToAction(nameof(Index));
                }

                return View(model);

            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred while trying to edit product with ID {ProductId}", id);

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExerciseCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool isSuccess = await exerciseService.EditExerciseAsync(model);

                if (!isSuccess)
                {
                    TempData[ErrorMessageKey] = "Error occured while editing the exercise!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Exercise updated successfully!";
                }

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Unexpected error occured while editing the exercise! Please contact the developer team.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrRestoreAsync(Guid id)
        {
            try
            {
                Tuple<bool,string> results = await exerciseService.DeleteOrRestoreAsync(id);

                bool isSuccess = results.Item1;
                string action = results.Item2;

                if (!isSuccess)
                {
                    TempData[ErrorMessageKey] = $"Error occured while trying to {action} exercise!";
                }
                else
                {
                    TempData[SuccessMessageKey] = $"The {action} was successful!";
                }

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Unexpected error occured!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
