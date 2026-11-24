using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITaskTagRepo
    {
        Task<List<TaskTag>> GetForUserAsync(string userId);
        Task AddAsync(TaskTag tag);
    }
}