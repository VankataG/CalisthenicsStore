using System.Runtime.CompilerServices;
using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;
using MockQueryable;
using Moq;
using NUnit.Framework;

namespace CalisthenicsStore.Tests.ServiceTests
{
    [TestFixture]
    public class ExerciseManagementServiceTests
    {
        private Mock<IExerciseRepository> exerciseRepositoryMock;

        private IExerciseManagementService exerciseService;

        [SetUp]
        public void SetUp()
        {
            this.exerciseRepositoryMock = new Mock<IExerciseRepository>();
            this.exerciseService = new ExerciseManagementService(exerciseRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task GetExerciseBoardDataAsyncShouldReturnNullWithoutExercises()
        {
            IQueryable<Exercise> emptyExerciseList = new List<Exercise>().BuildMock();

            exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(emptyExerciseList);

            var result =await exerciseService.GetExerciseBoardDataAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetExerciseBoardDataAsyncShouldReturnCorrectData()
        {
            Guid correctGuid = Guid.NewGuid();
            IQueryable<Exercise> emptyExerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = correctGuid,
                    Name = "Test",
                    IsDeleted = false,
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test2",
                    IsDeleted = false,
                    Level = DifficultyLevel.Beginner
                }
            }.BuildMock();

            exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(emptyExerciseList);

            var result = await exerciseService.GetExerciseBoardDataAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().IsDeleted, Is.False);
            Assert.That(result.First().Name, Is.EqualTo("Test"));
        }
    }
}
