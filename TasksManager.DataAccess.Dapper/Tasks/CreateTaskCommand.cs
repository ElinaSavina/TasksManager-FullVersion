using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TasksManager.DataAccess.Interfaces.Tasks;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;
using TasksManager.ViewModel.Tasks;
using TaskStatus = TasksManager.Entities.TaskStatus;

namespace TasksManager.DataAccess.Dapper.Tasks
{
    internal class CreateTaskCommand : ICreateTaskCommand
    {
        private readonly IConnectionFactory _connection;

        public CreateTaskCommand(IConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<TaskResponse> ExecuteAsync(CreateTaskRequest request)
        {
            using (var con = _connection.GetConnection())
            {
                var querySelectProject = "SELECT * from Projects where Id=@Id";
                var queryInsertTask =
                    @"insert into Tasks (Name, Description, DueDate,ProjectId,Status,CreateDate) values(@Name, @Description, @DueDate, @ProjectId,@Status,@CreateDate);
                                        SELECT CAST(SCOPE_IDENTITY() as int)";
                var queryInsertTag = @"insert into Tags (Name) values(@Name)
                                        SELECT CAST(SCOPE_IDENTITY() as int)";
                var queryInsertTagInTask = "insert into TagsInTasks(TaskId,TagId) values(@TaskId,@TagId)";
                var querySelectTag = "SELECT * from Tags where Name=@Name";
                int taskId;
                var project = con.Query<Project>(querySelectProject, new {Id = request.ProjectId}).FirstOrDefault();

                if (project == null) throw new ArgumentException("ProjectId incorrect", "projectId");

                using (var tr = con.BeginTransaction())
                {
                     var insertedTask = new Entities.Task
                     {
                         Name = request.Name,
                         Description = request.Description,
                         DueDate = request.DueDate,
                         ProjectId = request.ProjectId,
                         Status = TaskStatus.Created
                     };
                     taskId = con.Query<int>(queryInsertTask, insertedTask, tr).Single();

                     if (request.Tags.Length != 0)
                     {
                         foreach (var tag in request.Tags)
                         {
                             var tagExist = con.Query<Tag>(querySelectTag, new {Name = tag}, tr).SingleOrDefault();
                             var tagId = tagExist?.Id ?? con.Query<int>(queryInsertTag, tag, tr).Single();

                             con.Execute(queryInsertTagInTask, new {TaskId = taskId, TagId = tagId}, tr);
                         }
                     }
                     tr.Commit();

                     return new TaskResponse
                     {
                         Id = taskId,
                         Name = request.Name,
                         Description = request.Description,
                         DueDate = request.DueDate,
                         Tags = request.Tags,
                         Project = new ProjectResponse
                         {
                             Id = project.Id,
                             Name = project.Name,
                             Description = project.Description,
                             OpenTasksCount = project.OpenTasksCount
                         },
                         CreateDate = DateTime.Now
                     };
                }
            }
        }
    }
}

