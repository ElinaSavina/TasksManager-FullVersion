using System;
using AutoMapper;
using TasksManager.Entities;
using TasksManager.ViewModel.GeoObjects;
using TasksManager.ViewModel.Group;
using TasksManager.ViewModel.Projects;
using TasksManager.ViewModel.Tags;
using TasksManager.ViewModel.Tasks;
using TasksManager.ViewModel.Users;
using Task = TasksManager.Entities.Task;
using TaskStatus = TasksManager.ViewModel.TaskStatus;

namespace TasksManager
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            CreateMap<Project, ProjectResponse>();

            CreateMap<User, String>().ProjectUsing(s => s.Login);

            CreateMap<CreateProjectRequest, Project> ();

            CreateMap<UpdateProjectRequest, Project>();

            CreateMap<Task, TaskResponse>()
                .ForMember("Project",
                    opt => opt.MapFrom(p => Mapper.Map<Project, ProjectResponse>(p.Project)))
                 .ForMember("Status",
                    opt => opt.MapFrom(t => Mapper.Map<Entities.TaskStatus, TaskStatus>(t.Status)));
            CreateMap<TagsInTask, String>().ProjectUsing(o => o.Tag.Name);

            CreateMap<CreateTaskRequest, Task>()
                .ForMember("Status", opt => opt.UseValue(Entities.TaskStatus.Created));

            CreateMap<UpdateTaskRequest, Task>()
                .ForMember("Status", opt => opt.Condition(t => t.Status > 0 && (int)t.Status <= 4));
           
            CreateMap<String, Tag>()
                .ForMember("Name", opt => opt.MapFrom(t => t));

            CreateMap<String, TagsInTask>()
                .ForMember("Tag", opt => opt.MapFrom(t => t));

            CreateMap<Tag, TagResponse>();

            CreateMap<Group, GroupResponse>();

            CreateMap<CreateGroupRequest, Group>();

            CreateMap<UpdateGroupRequest, Group>();

            CreateMap<String, Student>()
                .ForMember("Name", opt => opt.MapFrom(n => n));

            CreateMap<Student, String>().ProjectUsing(s => s.Name);

            CreateMap<GeoObject, GeoObjectResponse>();
            CreateMap<GeoObject, String>().ProjectUsing(g => g.Name);

            CreateMap<AddGeoObjectRequest, GeoObject>();

            CreateMap<AuthRequest, User>();

            CreateMap<RegistrationRequest, User>();

            CreateMap<String, Role>()
                .ForMember("Name", opt => opt.MapFrom(n => n));
        }
    }
}



