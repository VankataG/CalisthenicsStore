using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;
using Microsoft.AspNetCore.Mvc;

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
    }
}
