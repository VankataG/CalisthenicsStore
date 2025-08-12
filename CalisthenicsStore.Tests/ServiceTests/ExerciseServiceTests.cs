using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Exercise;
using MockQueryable;
using Moq;
using NUnit.Framework;

namespace CalisthenicsStore.Tests.ServiceTests
{
    [TestFixture]
    public class ExerciseServiceTests
    {
        private Mock<IExerciseRepository> exerciseRepositoryMock;
        private IExerciseService exerciseService;

        [SetUp]
        public void SetUp()
        {
            this.exerciseRepositoryMock= new Mock<IExerciseRepository>();
            this.exerciseService = new ExerciseService(this.exerciseRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task GetAllExerciseAsyncShouldReturnEmptyListWithEmptyRepo()
        {
            List<Exercise> expectedEmptyExerciseList = new List<Exercise>();
            IQueryable<Exercise> expectedQueryable = expectedEmptyExerciseList.BuildMock();

            this.exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(expectedQueryable);

            IEnumerable<ExerciseViewModel> actualResult = await this.exerciseService.GetAllExercisesAsync();

            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Count(), Is.EqualTo(expectedQueryable.Count()));
        }

        [Test]
        public async Task GetAllExerciseAsyncShouldReturnMatchingCount()
        {
            List<Exercise> expectedEmptyExerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise1",
                    Description = "Description Test for Exercise1",
                    IsDeleted = false,
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise2",
                    Description = "Description Test for Exercise2",
                    IsDeleted = false,
                    Level = DifficultyLevel.Beginner
                }
            };
            IQueryable<Exercise> expectedQueryable = expectedEmptyExerciseList.BuildMock();

            this.exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(expectedQueryable);

            IEnumerable<ExerciseViewModel> actualResult = await this.exerciseService.GetAllExercisesAsync();

            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Count(), Is.EqualTo(expectedQueryable.Count()));
            foreach (ExerciseViewModel exerciseVm in actualResult)
            {
                Assert.That(exerciseVm.ImageUrl, Is.EqualTo("/images/no-image.jpg"));
            }
        }

        [Test]
        public async Task GetExerciseByLevelAsyncShouldReturnEmptyListWithNoMatch()
        {
            List<Exercise> expectedEmptyExerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise1",
                    Description = "Description Test for Exercise1",
                    IsDeleted = false,
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise2",
                    Description = "Description Test for Exercise2",
                    IsDeleted = false,
                    Level = DifficultyLevel.Insane
                }
            };
            IQueryable<Exercise> expectedQueryable = expectedEmptyExerciseList.BuildMock();

            this.exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(expectedQueryable);

            IEnumerable<ExerciseViewModel> actualResult = await this.exerciseService.GetExercisesByLevelAsync(DifficultyLevel.Beginner);

            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public async Task GetExerciseByLevelAsyncShouldReturnMatchingExercises()
        {
            List<Exercise> expectedEmptyExerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise1",
                    Description = "Description Test for Exercise1",
                    IsDeleted = false,
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise2",
                    Description = "Description Test for Exercise2",
                    IsDeleted = false,
                    Level = DifficultyLevel.Insane
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise3",
                    Description = "Description Test for Exercise3",
                    IsDeleted = false,
                    Level = DifficultyLevel.Beginner
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Exercise4",
                    Description = "Description Test for Exercise4",
                    IsDeleted = false,
                    Level = DifficultyLevel.Beginner
                }
            };
            IQueryable<Exercise> expectedQueryable = expectedEmptyExerciseList.BuildMock();

            this.exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(expectedQueryable);

            IEnumerable<ExerciseViewModel> actualResult = await this.exerciseService.GetExercisesByLevelAsync(DifficultyLevel.Beginner);

            foreach (ExerciseViewModel exerciseVm in actualResult)
            {
                Assert.That(exerciseVm.LevelEnum, Is.EqualTo(DifficultyLevel.Beginner));
            }
        }

        [Test]
        public async Task GetExerciseDetailsAsyncShouldReturnNullIfNotFound()
        {
            List<Exercise> expectedEmptyExerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.Parse("dc41f178-74d7-4554-bba1-f52eb2eccff8"),
                    Name = "Exercise1",
                    Description = "Description Test for Exercise1",
                    IsDeleted = false,
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = Guid.Parse("5d412737-ec4a-4604-bc4d-ff08581ca8f2"),
                    Name = "Exercise2",
                    Description = "Description Test for Exercise2",
                    IsDeleted = false,
                    Level = DifficultyLevel.Insane
                }
            };
            IQueryable<Exercise> expectedQueryable = expectedEmptyExerciseList.BuildMock();

            this.exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(expectedQueryable);

            ExerciseViewModel? actualResult = await this.exerciseService.GetExerciseDetailsAsync(Guid.NewGuid());

            Assert.That(actualResult, Is.Null);
        }

        [Test]
        public async Task GetExerciseDetailsAsyncShouldReturnExerciseMappedCorrectly()
        {
            Exercise expectedExercise = new Exercise()
            {
                Id = Guid.Parse("dc41f178-74d7-4554-bba1-f52eb2eccff8"),
                Name = "Exercise1",
                Description = "Description Test for Exercise1",
                IsDeleted = false,
                Level = DifficultyLevel.Advanced
            };

            this.exerciseRepositoryMock
                .Setup(er => er.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            ExerciseViewModel? actualResult = await this.exerciseService.GetExerciseDetailsAsync(expectedExercise.Id);

            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Id, Is.EqualTo(expectedExercise.Id));
            Assert.That(actualResult.Name, Is.EqualTo(expectedExercise.Name));
            Assert.That(actualResult.Description, Is.EqualTo(expectedExercise.Description));
            Assert.That(actualResult.ImageUrl, Is.EqualTo(expectedExercise.ImageUrl));
            Assert.That(actualResult.LevelEnum, Is.EqualTo(expectedExercise.Level));
        }
    }
}
