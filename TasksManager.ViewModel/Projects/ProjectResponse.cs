namespace TasksManager.ViewModel.Projects
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] RowVersion { get; set; }
        public int OpenTasksCount { get; set; }
        public string User { get; set; }
    }
}
