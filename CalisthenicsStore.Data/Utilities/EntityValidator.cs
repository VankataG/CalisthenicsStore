using System.ComponentModel.DataAnnotations;

using CalisthenicsStore.Data.Utilities.Interfaces;

namespace CalisthenicsStore.Data.Utilities
{
    public class EntityValidator : IValidator
    {
        public bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

    }
}
