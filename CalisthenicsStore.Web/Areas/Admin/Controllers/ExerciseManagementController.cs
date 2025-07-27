using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;
using CalisthenicsStore.ViewModels.Exercise;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class ExerciseManagementController : BaseAdminController
    {
        private readonly IExerciseManagementService exerciseService;

        public ExerciseManagementController(IExerciseManagementService exerciseService)
        {
            this.exerciseService = exerciseService;
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

            await exerciseService.AddExerciseAsync(model);
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
                    //TODO: Add ILogger
                    //logger.LogWarning("Attempted to edit product with ID {ProductId}, but it was not found.", id);

                    return RedirectToAction(nameof(Index));
                }
                else
                {


                    return View(model);
                }
            }
            catch (Exception e)
            {
                //TODO: Add ILogger
                //logger.LogError(ex, "Error occurred while trying to edit product with ID {ProductId}", id);

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

            await exerciseService.EditExerciseAsync(model);
            return RedirectToAction(nameof(Index), new { id = model.Id });
        }
    }
}
