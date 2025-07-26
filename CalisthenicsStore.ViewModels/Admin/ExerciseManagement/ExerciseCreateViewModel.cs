
using System.ComponentModel.DataAnnotations;

using CalisthenicsStore.Common.Enums;
using static CalisthenicsStore.Common.Constants.Exercise;
using static CalisthenicsStore.Common.ErrorMessages.Exercise;

namespace CalisthenicsStore.ViewModels.Admin.ExerciseManagement
{
    public class ExerciseCreateViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = NameRequiredError)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthError)]
        public string Name { get; set; } = null!;

        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthError)]
        public string? Description { get; set; }

        [Url(ErrorMessage = ImageUrlInvalidError)]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthError)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = LevelRequiredError)]
        public DifficultyLevel Level { get; set; }
    }
}
