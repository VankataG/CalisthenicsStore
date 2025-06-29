﻿using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Exercise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
