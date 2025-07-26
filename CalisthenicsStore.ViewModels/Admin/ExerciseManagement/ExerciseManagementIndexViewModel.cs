namespace CalisthenicsStore.ViewModels.Admin.ExerciseManagement
{
    public class ExerciseManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Level { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
