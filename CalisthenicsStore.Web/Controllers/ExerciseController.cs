using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Exercise;
using CalisthenicsStore.Services;
using CalisthenicsStore.ViewModels.Product;


namespace CalisthenicsStore.Web.Controllers
{
    public class ExerciseController(IExerciseService exerciseService) : BaseController
    {

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var exercises = await exerciseService.GetAllExercisesAsync();

            return View(exercises);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Level(DifficultyLevel level)
        {
            var exercises = await exerciseService.GetExercisesByLevelAsync(level);
            
            return View("Index", exercises);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            ExerciseViewModel? exercise = await exerciseService.GetExerciseDetailsAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }



        [HttpGet]
        public IActionResult Create()
        {
            ExerciseInputModel model = new ExerciseInputModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExerciseInputModel model)
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
                ExerciseInputModel? model = await exerciseService.GetEditableExerciseAsync(id);

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
        public async Task<IActionResult> Edit(ExerciseInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await exerciseService.EditExerciseAsync(model);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
    }
}
