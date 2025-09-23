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

            var result = await exerciseService.GetExerciseBoardDataAsync();
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
                    IsDeleted = true,
                    Level = DifficultyLevel.Beginner
                }
            }.BuildMock();

            exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(emptyExerciseList);

            IEnumerable<ExerciseManagementIndexViewModel> result = await exerciseService.GetExerciseBoardDataAsync();
            List<ExerciseManagementIndexViewModel> listResult = result.ToList();
            ExerciseManagementIndexViewModel resultModel = listResult[0];
            Assert.That(listResult, Is.Not.Empty);
            Assert.That(listResult.Count, Is.EqualTo(2));
            Assert.That(resultModel, Is.Not.Null);
            Assert.That(resultModel.Id, Is.EqualTo(correctGuid.ToString()));
            Assert.That(resultModel.Level, Is.EqualTo(DifficultyLevel.Advanced.ToString()));
            Assert.That(resultModel.IsDeleted, Is.False);
            Assert.That(resultModel.Name, Is.EqualTo("Test"));
        }

        [Test]
        public async Task AddExerciseAsyncShouldReturnFalse()
        {
            exerciseRepositoryMock
                .Setup(er => er.AddAsync(It.IsAny<Exercise>()))
                .ReturnsAsync(false);

            ExerciseCreateViewModel model = new ExerciseCreateViewModel()
            {
                Name = "TestModel",
                Level = DifficultyLevel.Insane,

            };

            bool result = await exerciseService.AddExerciseAsync(model);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AddExerciseAsyncShouldReturnTrue()
        {
            Exercise? createdExercise = null;
            exerciseRepositoryMock
                .Setup(er => er.AddAsync(It.IsAny<Exercise>()))
                .Callback<Exercise>(e => createdExercise = e)
                .ReturnsAsync(true);

            string correctName = "TestModel";
            string correctDesc = "TestDescription";

            ExerciseCreateViewModel model = new ExerciseCreateViewModel()
            {
                Name = correctName,
                Description = correctDesc,
                Level = DifficultyLevel.Insane,
            };

            bool result = await exerciseService.AddExerciseAsync(model);

            Assert.That(result, Is.True);
            Assert.That(createdExercise, Is.Not.Null);
            Assert.That(createdExercise!.Name, Is.EqualTo(correctName));
            Assert.That(createdExercise.Description, Is.EqualTo(correctDesc));
            Assert.That(createdExercise.IsDeleted, Is.EqualTo(false));
            Assert.That(createdExercise.Level, Is.EqualTo(DifficultyLevel.Insane));
            Assert.That(createdExercise.ImageUrl, Is.EqualTo("/images/no-image.jpg"));

        }

        [Test]
        public async Task GetEditableExerciseAsyncShouldReturnCorrectExercise()
        {
            Guid correctGuid = Guid.NewGuid();
            IQueryable<Exercise> exerciseList = new List<Exercise>()
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
                    IsDeleted = true,
                    Level = DifficultyLevel.Beginner
                }
            }.BuildMock();

            exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(exerciseList);

            ExerciseCreateViewModel? result = await exerciseService.GetEditableExerciseAsync(correctGuid);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(correctGuid));
            Assert.That(result.Name, Is.EqualTo("Test"));
            Assert.That(result.Level, Is.EqualTo(DifficultyLevel.Advanced));
            Assert.That(result.ImageUrl, Is.EqualTo(null));
        }

        [Test]
        public async Task GetEditableExerciseAsyncShouldReturnNull()
        {
            IQueryable<Exercise> emptyExerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    IsDeleted = false,
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test2",
                    IsDeleted = true,
                    Level = DifficultyLevel.Beginner
                }
            }.BuildMock();

            exerciseRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(emptyExerciseList);

            ExerciseCreateViewModel? result = await exerciseService.GetEditableExerciseAsync(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task EditExerciseAsyncShouldReturnTrueWhenExistAndUpdate()
        {
            Guid correctGuid = Guid.NewGuid();

            IQueryable<Exercise> exerciseList = new List<Exercise>()
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
                .Returns(exerciseList);


            string changedName = "ChangedTest";
            DifficultyLevel changedLevel = DifficultyLevel.Insane;

            ExerciseCreateViewModel model = new ExerciseCreateViewModel()
            {
                Id = correctGuid,
                Name = changedName,
                Level = changedLevel
            };

            Exercise? updatedExercise = null;

            exerciseRepositoryMock
                .Setup(er => er.UpdateAsync(It.IsAny<Exercise>()))
                .Callback<Exercise>(e => updatedExercise = e)
                .ReturnsAsync(true);

            bool result = await exerciseService.EditExerciseAsync(model);
            
            Assert.That(result, Is.True);
            Assert.That(updatedExercise, Is.Not.Null);
            Assert.That(updatedExercise!.Id, Is.EqualTo(correctGuid));
            Assert.That(updatedExercise.Name, Is.EqualTo(changedName));
            Assert.That(updatedExercise.Level, Is.EqualTo(changedLevel));
        }

        [Test]
        public async Task EditExerciseAsyncShouldReturnFalseWhenExistAndUpdateFail()
        {
            Guid correctGuid = Guid.NewGuid();

            IQueryable<Exercise> exerciseList = new List<Exercise>()
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
                .Returns(exerciseList);


            string changedName = "ChangedTest";
            DifficultyLevel changedLevel = DifficultyLevel.Insane;

            ExerciseCreateViewModel model = new ExerciseCreateViewModel()
            {
                Id = correctGuid,
                Name = changedName,
                Level = changedLevel
            };

            exerciseRepositoryMock
                .Setup(er => er.UpdateAsync(It.IsAny<Exercise>()))
                .ReturnsAsync(false);

            bool result = await exerciseService.EditExerciseAsync(model);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditExerciseAsyncShouldReturnFalseWhenExerciseIsDeleted()
        {
            Guid correctGuid = Guid.NewGuid();

            IQueryable<Exercise> exerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = correctGuid,
                    Name = "Test",
                    IsDeleted = true,
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
                .Returns(exerciseList);


            string changedName = "ChangedTest";
            DifficultyLevel changedLevel = DifficultyLevel.Insane;

            ExerciseCreateViewModel model = new ExerciseCreateViewModel()
            {
                Id = correctGuid,
                Name = changedName,
                Level = changedLevel
            };

            bool result = await exerciseService.EditExerciseAsync(model);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditExerciseAsyncShouldReturnFalseWhenExerciseDoNotExist()
        {
            IQueryable<Exercise> exerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    IsDeleted = true,
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
                .Returns(exerciseList);


            string changedName = "ChangedTest";
            DifficultyLevel changedLevel = DifficultyLevel.Insane;

            ExerciseCreateViewModel model = new ExerciseCreateViewModel()
            {
                Id = Guid.NewGuid(),
                Name = changedName,
                Level = changedLevel
            };

            bool result = await exerciseService.EditExerciseAsync(model);

            Assert.That(result, Is.False);
        }


        [Test]
        public async Task DeleteOrRestoreAsyncShouldReturnFalseWhenExerciseDoNotExist()
        {
            IQueryable<Exercise> exerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    IsDeleted = true,
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
                .Returns(exerciseList);

            Tuple<bool,string> result = await exerciseService.DeleteOrRestoreAsync(Guid.NewGuid());

            Assert.That(result.Item1, Is.False);
            Assert.That(result.Item2, Is.Empty);
        }

        [Test]
        public async Task DeleteOrRestoreAsyncShouldReturnTrueWhenExerciseExistAndIsActive()
        {
            Guid correctGuid = Guid.NewGuid();
            IQueryable<Exercise> exerciseList = new List<Exercise>()
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
                .Returns(exerciseList);

            Tuple<bool, string> result = await exerciseService.DeleteOrRestoreAsync(correctGuid);

            Assert.That(result.Item1, Is.True);
            Assert.That(result.Item2, Is.EqualTo("delete"));
        }

        [Test]
        public async Task DeleteOrRestoreAsyncShouldReturnTrueWhenExerciseExistAndIsDeleted()
        {
            Guid correctGuid = Guid.NewGuid();
            IQueryable<Exercise> exerciseList = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = correctGuid,
                    Name = "Test",
                    IsDeleted = true,
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
                .Returns(exerciseList);

            Tuple<bool, string> result = await exerciseService.DeleteOrRestoreAsync(correctGuid);

            Assert.That(result.Item1, Is.True);
            Assert.That(result.Item2, Is.EqualTo("restore"));
        }
    }
}
