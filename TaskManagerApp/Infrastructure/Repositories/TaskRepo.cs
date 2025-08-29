using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class TaskRepo : ITaskRepo
    {
        private readonly ApplicationDBContext _db;
        public TaskRepo(ApplicationDBContext db) => _db = db;

        public async Task<List<TaskItem>> GetTasksForUserAsync(string userId, bool? isCompleted = null) =>
            await _db.Tasks
                .Where(t => t.UserId == userId && (isCompleted == null || t.IsCompleted == isCompleted))
                .ToListAsync();

        public async Task AddAsync(TaskItem task)
        {
            await _db.Tasks .AddAsync(task);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int taskId)
        {
            var task = await _db.Tasks.FindAsync(taskId);
            if (task != null)
            {
                _db.Tasks.Remove(task);
                await _db.SaveChangesAsync();
            }
        }
    }
}
