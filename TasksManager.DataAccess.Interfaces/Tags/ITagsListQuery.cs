﻿using System.Threading.Tasks;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;

namespace TasksManager.DataAccess.Interfaces.Tags
{
    public interface ITagsListQuery
    {
        Task<ListResponse<TagResponse>> RunAsync(TagFilter filter, ListOptions options);
    }
}
