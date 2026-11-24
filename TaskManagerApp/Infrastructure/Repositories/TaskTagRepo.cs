using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskTagRepo : ITaskTagRepo
    {
        private readonly ApplicationDBContext _db;

        public TaskTagRepo(ApplicationDBContext db) => _db = db;

        public Task<List<TaskTag>> GetForUserAsync(string userId) =>
            _db.TaskTags
                .Where(tag => tag.UserId == userId)
                .OrderBy(tag => tag.Name)
                .ToListAsync();

        public async Task AddAsync(TaskTag tag)
        {
            await _db.TaskTags.AddAsync(tag);
            await _db.SaveChangesAsync();
        }
    }
}