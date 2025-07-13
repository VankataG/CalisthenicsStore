using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Exercise;


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
        public async Task<IActionResult> Details(int id)
        {
            ExerciseViewModel? exercise = await exerciseService.GetExerciseDetailsAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }
    }
}
