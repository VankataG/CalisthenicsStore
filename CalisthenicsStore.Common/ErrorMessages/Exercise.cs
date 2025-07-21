
using static CalisthenicsStore.Common.Constants.Exercise;


namespace CalisthenicsStore.Common.ErrorMessages
{
    public static class Exercise
    {
        public const string NameRequiredError = "Exercise name is required.";

        public const string NameMaxLengthError = "Name is too long.";

        public const string DescriptionMaxLengthError = "Description is too long.";

        public const string ImageUrlInvalidError = "Please enter a valid URL.";

        public const string ImageUrlMaxLengthError = "IRL is too long.";

        public const string LevelRequiredError = "Please select a difficulty level.";
    }
}
